namespace GitBuildInfo.SourceGenerator
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Source Generator for dumping git build information into a assembly level attribute on the compilation.
    /// </summary>
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation is not CSharpCompilation)
            {
                return;
            }

            _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var projectdir);
            var gitBuildInfoJsonFile = context.AdditionalFiles
                .FirstOrDefault(af => string.Equals(Path.GetFileName(af.Path), "GitBuildInfo.json", StringComparison.OrdinalIgnoreCase));
            var gitInfoJsonFile = context.AdditionalFiles
                .FirstOrDefault(af => string.Equals(Path.GetFileName(af.Path), "GitInfo.json", StringComparison.OrdinalIgnoreCase));
            if (gitBuildInfoJsonFile is null || gitInfoJsonFile is null)
            {
                return;
            }

            var jsonStr = gitBuildInfoJsonFile.GetText(context.CancellationToken)!.ToString();
            var options = JsonSerializer.Deserialize<GeneratorOptions>(
                jsonStr,
                new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            var jsonStr2 = gitInfoJsonFile.GetText(context.CancellationToken)!.ToString();
            var gitInfo = JsonSerializer.Deserialize<GitInfo>(
                jsonStr2,
                new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            if (string.IsNullOrEmpty(options!.AssemblyType))
            {
                throw new GenerationFailedException("AssemblyType should not be null or an empty string.");
            }

            var splitted = options!.AssemblyType!.Contains(".") ? options!.AssemblyType.Split('.') : new string[] { };
            var splitted2 = new Span<string>(splitted, 0, splitted.Length - 1);
            var splittedLen = splitted.Length;
            var usingStr = new StringBuilder();
            var gitinformationNamespace = "Elskom.Generic.Libs";
            foreach (var value in splitted)
            {
                // skip the last value.
                if (value != splitted[splittedLen - 1])
                {
                    _ = usingStr.Append(value != splitted[splittedLen - 2] ? $"{value}." : value);
                }
            }

            var generated = GenerateCode(
                options,
                splitted2.ToArray(),
                gitinformationNamespace,
                gitInfo!.GitHead!,
                gitInfo.CommitHash!,
                gitInfo.GitBranch!,
                splittedLen > 0 ? splitted[splittedLen - 1] : options!.AssemblyType);
            context.AddSource("GitAssemblyInfo.g.cs", SourceText.From(generated.ToFullString(), Encoding.UTF8));
        }

        private static CompilationUnitSyntax GenerateCode(GeneratorOptions options, string[] usings, string originalnamespace, string arg1, string arg2, string arg3, string typeName)
            => SyntaxFactory.CompilationUnit().WithUsings(
                SyntaxFactory.List(
                    string.Equals(string.Join(".", usings), originalnamespace, StringComparison.Ordinal)
                    ? new UsingDirectiveSyntax[] {
                        AddUsing(new string[] { "Elskom", "Generic", "Libs" }, true)
                    }
                    : new UsingDirectiveSyntax[] {
                        AddUsing(new string[] { "Elskom", "Generic", "Libs" }, true),
                        AddUsing(usings, false)
                    }))
            .WithAttributeLists(
                SyntaxFactory.SingletonList(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("GitInformationAttribute"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                                        MakeAttributeArgumentList(options, new string[] { arg1, arg2, arg3 }, typeName))))))
                    .WithOpenBracketToken(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(SyntaxFactory.LineFeed),
                            SyntaxKind.OpenBracketToken,
                            SyntaxFactory.TriviaList()))
                    .WithTarget(
                        SyntaxFactory.AttributeTargetSpecifier(SyntaxFactory.Token(SyntaxKind.AssemblyKeyword))
                        .WithColonToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.ColonToken,
                                SyntaxFactory.TriviaList(SyntaxFactory.Space))))
                    .WithCloseBracketToken(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.CloseBracketToken,
                            SyntaxFactory.TriviaList(SyntaxFactory.LineFeed)))));

        private static UsingDirectiveSyntax AddUsing(string[] strings, bool autogeneratedheader)
        {
            NameSyntax? qualifiedName = null;
            for (var index = 0; index < strings.Length; index++)
            {
                if (index == 0 && strings.Length > 1)
                {
                    qualifiedName = SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName(strings[index]),
                        SyntaxFactory.IdentifierName(strings[index + 1]));
                    index++;
                }
                else
                {
                    qualifiedName = strings.Length == 1
                        ? SyntaxFactory.IdentifierName(strings[index])
                        : SyntaxFactory.QualifiedName(qualifiedName!, SyntaxFactory.IdentifierName(strings[index]));
                }
            }

            return SyntaxFactory.UsingDirective(qualifiedName!)
                .WithUsingKeyword(SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(autogeneratedheader ? new[] { SyntaxFactory.Comment("// <autogenerated/>"), SyntaxFactory.LineFeed } : Array.Empty<SyntaxTrivia>()),
                    SyntaxKind.UsingKeyword,
                    SyntaxFactory.TriviaList(SyntaxFactory.Space)))
                .WithSemicolonToken(SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.SemicolonToken,
                    SyntaxFactory.TriviaList(SyntaxFactory.LineFeed)));
        }

        private static SyntaxNodeOrToken[] MakeAttributeArgumentList(GeneratorOptions options, string[] args, string typeName)
        {
            var lst = new SyntaxNodeOrToken[7];
            var lstIndex = 0;
            foreach (var arg in args)
            {
                lst[lstIndex] = SyntaxFactory.AttributeArgument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(arg)));
                lstIndex++;
                lst[lstIndex] = SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.CommaToken,
                    SyntaxFactory.TriviaList(SyntaxFactory.Space));
                lstIndex++;
            }

            lst[lstIndex] = SyntaxFactory.AttributeArgument(
                SyntaxFactory.TypeOfExpression(
                    options.IsGeneric
                    ? SyntaxFactory.GenericName(typeName)
                    : SyntaxFactory.IdentifierName(typeName)));
            return lst;
        }
    }
}
