using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace WinMergeExtensionInstaller
{
	class Program
	{
		const string DirName = @"C:\WinMergeExtension\";
		const string FilePrName = @"PrepareWinMerge.bat";
		const string FilePrDebugModeName = @"PrepareWinMergeDebugMode.bat";
		const string FileStName = @"StartWinMerge.bat";
		const string FileSettingsName = @"Settings.txt";
		const string FileExtName = @"WinMergeExtension.exe";
		const string PrCode = @"start " + DirName + FileExtName + " %1";
		const string StCode = @"start WinMergeU.exe %3 /maximize %1 %2";
		const string SentToPath = @"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\SendTo\";
		
		static void Main(string[] args)
		{
			if(Directory.Exists(DirName))
				Directory.Delete(DirName,true);
			Directory.CreateDirectory(DirName);

			StreamWriter outPrFile = new StreamWriter(DirName + FilePrName);
			outPrFile.WriteLine(PrCode);
			outPrFile.Close();

			outPrFile = new StreamWriter(DirName + FilePrDebugModeName);
			outPrFile.WriteLine(PrCode+" true");
			outPrFile.Close();

			outPrFile = new StreamWriter(DirName + FileStName);
			outPrFile.WriteLine(StCode);
			outPrFile.Close();

			outPrFile = new StreamWriter(DirName + FileSettingsName);
			outPrFile.WriteLine(GetSettingsTemplate());
			outPrFile.Close();

			File.Copy(DirName + FilePrName, 
				string.Format(SentToPath, Environment.UserName) + FilePrName, true);
			File.Copy(DirName + FilePrDebugModeName, 
				string.Format(SentToPath, Environment.UserName) + FilePrDebugModeName, true);
			File.Copy(Directory.GetCurrentDirectory()+ "\\" + FileExtName, DirName + FileExtName);
			

			Environment.Exit(0);
		}

		private static string GetSettingsTemplate()
		{
			StringBuilder res = new StringBuilder();
			
			res.AppendLine(new string('*',27));
			res.AppendLine("WinMergeExtension Settings");
			res.AppendLine(new string('*',27));
			res.AppendLine("** Here you can set default location of your branches");
			res.AppendLine("BrPath=C:\\Sources\\");
			res.AppendLine(new string('*', 27));

			return res.ToString();
		}
	}
}
