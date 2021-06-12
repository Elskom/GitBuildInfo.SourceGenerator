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
        /// Gets or sets the directory that contains the project.
        /// </summary>
        [Required]
        public string ProjectDir { get; set; }

        /// <summary>
        /// Gets or sets the git head information.
        /// </summary>
        [Output]
        public string GitHead { get; set; }

        /// <summary>
        /// Gets or sets the git commit hash.
        /// </summary>
        [Output]
        public string CommitHash { get; set; }

        /// <summary>
        /// Gets or sets the current git branch information.
        /// </summary>
        [Output]
        public string GitBranch { get; set; }

        /// <inheritdoc/>
        public override bool Execute()
        {
            this.GitHead = this.RunGit("describe --all --always --dirty");
            this.CommitHash = this.RunGit("rev-parse --short HEAD");
            this.GitBranch = this.RunGit("name-rev --name-only HEAD");
            this.Log.LogMessage(MessageImportance.High, "Getting build info from git");
            return true;
        }

        private string RunGit(string arguments)
        {
            using var pro1 = new Process();
            pro1.StartInfo.FileName = "git";
            pro1.StartInfo.Arguments = arguments;
            pro1.StartInfo.RedirectStandardOutput = true;
            pro1.StartInfo.UseShellExecute = false;
            pro1.StartInfo.CreateNoWindow = true;
            pro1.StartInfo.WorkingDirectory = Path.GetFullPath(this.ProjectDir).Replace(Path.GetFileName(this.ProjectDir), string.Empty);
            try
            {
                _ = pro1.Start();
                var git_out = pro1.StandardOutput.ReadToEnd();
                pro1.WaitForExit();
                // handle all cases of possible endlines.
                git_out = git_out.Replace("\r\n", string.Empty);
                git_out = git_out.Replace("\n", string.Empty);
                git_out = git_out.Replace("\r", string.Empty);
                return git_out;
            }
            catch (Win32Exception)
            {
                return "Not a git clone or git is not in Path.";
            }
        }
    }
}
