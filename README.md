# GitBuildInfo.SourceGenerator
Source Generator for dumping the git branch information, commit hash, and if the working tree is dirty or clean on projects that install this and applies them as an assembly level attribute.

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d13da68cd3784f1486af29432e75e707)](https://www.codacy.com/gh/Elskom/GitBuildInfo.SourceGenerator/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Elskom/GitBuildInfo.SourceGenerator&amp;utm_campaign=Badge_Grade)
[![Codacy Coverage Badge](https://app.codacy.com/project/badge/Coverage/d13da68cd3784f1486af29432e75e707)](https://www.codacy.com/gh/Elskom/GitBuildInfo.SourceGenerator/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Elskom/GitBuildInfo.SourceGenerator&amp;utm_campaign=Badge_Coverage)

| Package | Version |
|:-------:|:-------:|
| GitBuildInfo.SourceGenerator | [![NuGet Badge](https://buildstats.info/nuget/GitBuildInfo.SourceGenerator?includePreReleases=true)](https://www.nuget.org/packages/GitBuildInfo.SourceGenerator/) |

## Usage

1. Install this package into your project with:
```xml
<PackageReference Include="GitBuildInfo.SourceGenerator" IsImplicitlyDefined="true" Version="*-*">
  <PrivateAssets>all</PrivateAssets>
</PackageReference>
```
2. Set the following msbuild properties in your project (or if you have all project file property settings stored in a project specific ``Directory.Build.props``) file:
  * ``<GitBuildInfoIsGeneric></GitBuildInfoIsGeneric>`` (Optional, default is false)
  * ``<GitBuildInfoAssemblyType></GitBuildInfoAssemblyType>`` (Required, Note: Do not include anything before the type name like a fully qualified namespace or any ``.``'s. For that you need to set ``RootNamespace`` below)
  * ``<RootNamespace></RootNamespace>`` (Required unless you want the type to be assumed to be in the ``Elskom.Generic.Libs`` namespace by the generator)
3. The generator package should now run a build task to grab information prior to it executing the generator.
