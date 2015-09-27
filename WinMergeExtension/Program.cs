using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinMergeExtension
{
	class Program
	{
		const string DebugIdentifier = "true";
		const string FilePath = @"C:\WinMergeExtension\StartWinMerge.bat";
		const string SettingsFile = @"C:\WinMergeExtension\Settings.txt";
		const string BranchParName = "BrPath";
		const char SplitterSettings = '=';
		const char SplitterPath = '\\';

		static bool isDebugMode = false;
		static string additionalArgs = string.Empty;
		static string branchesPath = string.Empty;
		static string inputArgs = string.Empty;
		static string secondFile = string.Empty;
		static string curBranch = string.Empty;
		static List<string> branchNames = new List<string>();
		static ProcessStartInfo processInfo;
		static Process process;
		static StreamReader settings;

		static void Main(string[] args)
		{
			if (args.Length >= 1)
			{
				CheckMode(args);

				GetSettings();

				if (branchesPath == null || string.Empty.Equals(branchesPath))
					return;

				inputArgs = args[0] + " ";

				GetBranches();

				Chooser chooseBranch = new Chooser(branchNames);

				chooseBranch.ShowDialog();

				if (chooseBranch.DialogResult == System.Windows.Forms.DialogResult.OK)
					inputArgs += branchesPath 
						+ chooseBranch.SelectedElement 
						+ inputArgs.Remove(0, inputArgs.IndexOf(curBranch) + curBranch.Length) 
						+ " ";
				else
					return;

				StartWinMerge();
			}
		}

		private static void StartWinMerge()
		{
			processInfo = new ProcessStartInfo(FilePath, inputArgs + additionalArgs);
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

				MessageBox.Show("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
				MessageBox.Show("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
				MessageBox.Show("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
			}

			process.Close();
		}

		private static void GetBranches()
		{
			if (Directory.GetDirectories(branchesPath) != null)
				foreach (string path in Directory.GetDirectories(branchesPath))
				{
					if (inputArgs.Contains(path))
						curBranch = path.Split(SplitterPath).Last();
					else
						branchNames.Add(path.Split(SplitterPath).Last());
				}			
		}

		private static void GetSettings()
		{
			settings = new StreamReader(SettingsFile);

			do
			{
				string line = settings.ReadLine();

				if (line.StartsWith(BranchParName))
					branchesPath = line.Split(SplitterSettings)[1];
			} while (!settings.EndOfStream);
		}

		private static void CheckMode(string[] args)
		{
			foreach (string a in args)
			{
				if (DebugIdentifier.Equals(a))
					isDebugMode = true;
				inputArgs += a + " ";
			}

			if (isDebugMode)
				MessageBox.Show(inputArgs);
		}
	}
}
