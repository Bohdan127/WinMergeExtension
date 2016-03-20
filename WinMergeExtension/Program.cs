using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WpfChooser;
using WpfChooser.Enums;
using WpfChooser.Interfaces;

namespace WinMergeExtension
{
    class Program
    {
        const string DebugIdentifier = "true";
        const string CreateIdentifier = "create";
        const string FilePath = @"C:\WinMergeExtension\StartWinMerge.bat";
        const string SettingsFile = @"C:\WinMergeExtension\Settings.txt";
        const string BranchParName = "BrPath";
        const char SplitterSettings = '=';
        const char SplitterVersions = '.';
        const char SplitterBytes = ',';
        const char SplitterPath = '\\';
        const string BytesVariable = "string WinMergeBytes";
        const string BytesVariableWpf = "string WpfBytes";
        const string FileExtName = "WinMergeExtension.exe";
        const string FileWpfExtName = "WpfChooser.exe";
        const string DialogTitle = "Choose WinMergeExtentionInstaller 'Program.cs' file";
        const string Space = " ";
        const string DefaultUpdatePathName = "UpdateUrl";
        const string DefaultUpdateFileName = "UpdateFile";
        const string DefaultUpdatesCheckName = "CheckUpdates";
        const string DefaultAutoUpdateName = "AutoUpdate";

        static bool isDebugMode = false;
        static bool checkForUpdates = false;
        static bool automaticallyUpdate = false;
        static string branchesPath = string.Empty;
        static string updatePath = string.Empty;
        static string updateFile = string.Empty;
        static string inputArgs = string.Empty;
        static string secondFile = string.Empty;
        static string curBranch = string.Empty;
        static string bytesLineTemplate = "string WinMergeBytes = \"{0}\";";
        static string bytesLineTemplateWpf = "string WpfBytes = \"{0}\";";
        static List<string> branchNames = new List<string>();
        static ProcessStartInfo processInfo;
        static Process process;
        static StreamReader settings;
        static IMessageBox messageBox;

        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            //args[0] = @"C:\Sources\1\1.txt";//for test in VS
            //args[1] = "true";//for test in VS

            CheckMode(args);
            GetSettings();

            if (checkForUpdates)
                CheckForUpdates();

            if (args.Length >= 1)
            {
                if (CreateIdentifier.Equals(args[0]))
                {
                    CreateInstaller();
                    return;
                }

                if (branchesPath == null || string.Empty.Equals(branchesPath))
                {
                    if (isDebugMode)
                        MessageBox_Show("BranchesPath is null or empty", "Error");
                    return;
                }

                inputArgs = args[0] + Space;
                GetBranches();

                MainWindow chooseBranch = new MainWindow();
                chooseBranch.SetBranches(branchNames);
                chooseBranch.ShowDialog();

                if (chooseBranch.Selected)
                    inputArgs += branchesPath
                        + chooseBranch.SelectedElement
                        + inputArgs.Remove(0, inputArgs.IndexOf(curBranch) + curBranch.Length)
                        + Space;

                else
                    return;

                StartWinMerge();
            }
        }

        //todo really big and bad crutch, NEED TO FIX IT!!!!
        private static Result MessageBox_Show(string text, string title = null, Mode mode = Mode.Ok)
        {
            //not initialize by some DI and should be initialize here
            messageBox = new CustomMessageBox();
            return messageBox.Show(text, title, mode);
        }

        private static void CheckForUpdates()
        {
            try
            {
                bool exist = new System.IO.DirectoryInfo(updatePath).Exists;
                bool shouldUpdate = exist ? ShouldUpdate() : false;

                if (isDebugMode)
                {
                    MessageBox_Show(string.Format("{0} - Exists {1}", updatePath, exist));
                    MessageBox_Show(string.Format("Need Some Update - {0}", shouldUpdate));
                    MessageBox_Show(string.Format("Automatically Update - {0}", automaticallyUpdate));

                }

                if (exist && shouldUpdate)
                {
                    if (!automaticallyUpdate)
                        shouldUpdate = MessageBox_Show("New Update Available, should update now?"
                        , "New Version", Mode.OkCancel) == Result.Yes;
                    if (shouldUpdate)
                    {
                        if (isDebugMode)
                        {
                            MessageBox_Show(string.Format("Call Process.Start({0});", updatePath + updateFile));
                            MessageBox_Show("Exit for Update");
                        }
                        Process.Start(updatePath + updateFile);
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                if (isDebugMode)
                    MessageBox_Show(ex.Message, "Error");
                return;
            }
        }

        private static bool ShouldUpdate()
        {
            Assembly remoteAssembly = null;
            Assembly currAssembly = null;

            remoteAssembly = Assembly.LoadFrom(updatePath + updateFile);
            currAssembly = Assembly.GetExecutingAssembly();

            string[] remoteAppVersion = remoteAssembly.GetCustomAttributes(false)
                 .OfType<AssemblyFileVersionAttribute>().Single().Version.Split(SplitterVersions);
            string[] currAppVersion = currAssembly.GetCustomAttributes(false)
                .OfType<AssemblyFileVersionAttribute>().Single().Version.Split(SplitterVersions);

            for (int i = 0; i < remoteAppVersion.Count(); i++)
            {
                int remAppVer = Convert.ToInt32(remoteAppVersion[i]);
                int currAppVer = Convert.ToInt32(currAppVersion[i]);
                if (remAppVer < currAppVer)
                    break;
                else if (remAppVer > currAppVer)
                    return true;
            }

            return false;
        }

        private static void CreateInstaller()
        {


            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = DialogTitle;
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ReWriteInstaller(dialog.FileName, GetWinMergeBytes());
                    ReWriteInstallerWpf(dialog.FileName, GetWpfBytes());
                }
            }
        }

        private static string GetWinMergeBytes()
        {
            byte[] buffer = File.ReadAllBytes(Environment.CurrentDirectory + SplitterPath + FileExtName);
            string winMergeBytes = string.Empty;

            foreach (var b in buffer)
                winMergeBytes += b + SplitterBytes.ToString();

            return winMergeBytes.TrimEnd(SplitterBytes);
        }

        private static string GetWpfBytes()
        {
            byte[] buffer = File.ReadAllBytes(Environment.CurrentDirectory + SplitterPath + FileWpfExtName);
            string WpfBytes = string.Empty;

            foreach (var b in buffer)
                WpfBytes += b + SplitterBytes.ToString();

            return WpfBytes.TrimEnd(SplitterBytes);
        }

        private static void ReWriteInstaller(string path, string winMergeBytes)
        {
            StringBuilder content = new StringBuilder();

            using (StreamReader reader = new StreamReader(path))
            {
                do
                {
                    string line = reader.ReadLine();
                    if (!line.Contains(BytesVariable))
                        content.AppendLine(line);
                    else
                        content.AppendLine(string.Format(bytesLineTemplate, winMergeBytes));
                } while (!reader.EndOfStream);
            }

            File.WriteAllText(path, content.ToString());
        }

        private static void ReWriteInstallerWpf(string path, string winMergeBytes)
        {
            StringBuilder content = new StringBuilder();

            using (StreamReader reader = new StreamReader(path))
            {
                do
                {
                    string line = reader.ReadLine();
                    if (!line.Contains(BytesVariableWpf))
                        content.AppendLine(line);
                    else
                        content.AppendLine(string.Format(bytesLineTemplateWpf, winMergeBytes));
                } while (!reader.EndOfStream);
            }

            File.WriteAllText(path, content.ToString());
        }

        private static void StartWinMerge()
        {
            processInfo = new ProcessStartInfo(FilePath, inputArgs);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            if (isDebugMode)
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                int exitCode = process.ExitCode;

                MessageBox_Show("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
                MessageBox_Show("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
                MessageBox_Show("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            }

            process.Close();
        }

        private static void GetBranches()
        {
            try
            {
                if (Directory.GetDirectories(branchesPath) != null)
                    foreach (string path in Directory.GetDirectories(branchesPath))
                    {
                        if (inputArgs.Contains(path + "\\"))
                            curBranch = path.Split(SplitterPath).Last();
                        else
                            branchNames.Add(path.Split(SplitterPath).Last());
                    }
            }
            catch (Exception ex)
            {
                if (isDebugMode)
                    MessageBox_Show(ex.Message, "Error");
                return;
            }
        }

        private static void GetSettings()
        {
            try
            {
                settings = new StreamReader(SettingsFile);

                do
                {
                    string line = settings.ReadLine();

                    if (line.StartsWith(BranchParName))
                        branchesPath = line.Split(SplitterSettings)[1];
                    else if (line.StartsWith(DefaultUpdatePathName))
                        updatePath = line.Split(SplitterSettings)[1];
                    else if (line.StartsWith(DefaultUpdateFileName))
                        updateFile = line.Split(SplitterSettings)[1];
                    else if (line.StartsWith(DefaultUpdatesCheckName))
                        checkForUpdates = Convert.ToBoolean(line.Split(SplitterSettings)[1]);
                    else if (line.StartsWith(DefaultAutoUpdateName))
                        automaticallyUpdate = Convert.ToBoolean(line.Split(SplitterSettings)[1]);
                } while (!settings.EndOfStream);
            }
            catch (Exception ex)
            {
                if (isDebugMode)
                    MessageBox_Show(ex.Message, "Error");
                return;
            }
        }

        private static void CheckMode(string[] args)
        {
            foreach (string a in args)
            {
                if (DebugIdentifier.Equals(a))
                    isDebugMode = true;
                inputArgs += a + Space;
            }

            if (isDebugMode)
            {
                MessageBox_Show(inputArgs);
            }
        }
    }
}
