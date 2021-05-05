namespace GitBuildInfo
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// A MSBuild task that generates the msbuild information for an assembly.
    ///
    /// Note: use in the BeforeBuild target.
    /// </summary>
    public class GitInfoTask : Task
    {
        /// <summary>
        /// Gets or sets the generated output file path.
        /// </summary>
        [Required]
        public string OutputPath { get; set; }

        /// <inheritdoc/>
        public override bool Execute()
        {
            this.RunGit("describe --all --always --dirty", out var git_out1);
            this.RunGit("rev-parse --short HEAD", out var git_out2);
            this.RunGit("name-rev --name-only HEAD", out var git_out3);
            var outputData = $"{{{Environment.NewLine}  \"GitHead\": \"{git_out1}\",{Environment.NewLine}  \"CommitHash\": \"{git_out2}\",{Environment.NewLine}  \"GitBranch\": \"{git_out3}\"{Environment.NewLine}}}";
            // patch 112019: only print the getting build info from git message from the initial call to this task.
            // all other calls will not print anything to avoid spamming up the build output.
            try
            {
                if (!File.Exists(this.OutputPath) || (File.Exists(this.OutputPath) && !string.Equals(outputData, File.ReadAllText(this.OutputPath), StringComparison.Ordinal)))
                {
                    this.Log.LogMessage(MessageImportance.High, "Getting build info from git");
                    File.WriteAllText(this.OutputPath, outputData);
                }
            }
            catch (IOException)
            {
                // catch I/O error from being unable to open the file for checking it's contents.
            }
            return true;
        }

        private void RunGit(string arguments, out string git_out)
        {
            using var pro1 = new Process();
            pro1.StartInfo.FileName = "git";
            pro1.StartInfo.Arguments = arguments;
            pro1.StartInfo.RedirectStandardOutput = true;
            pro1.StartInfo.UseShellExecute = false;
            pro1.StartInfo.CreateNoWindow = true;
            pro1.StartInfo.WorkingDirectory = Path.GetFullPath(this.OutputPath).Replace(Path.GetFileName(this.OutputPath), string.Empty);
            try
            {
                _ = pro1.Start();
                git_out = pro1.StandardOutput.ReadToEnd();
                pro1.WaitForExit();
                // handle all cases of possible endlines.
                git_out = git_out.Replace("\r\n", string.Empty);
                git_out = git_out.Replace("\n", string.Empty);
                git_out = git_out.Replace("\r", string.Empty);
            }
            catch (Win32Exception)
            {
                git_out = "Not a git clone or git is not in Path.";
            }
        }
    }
}
