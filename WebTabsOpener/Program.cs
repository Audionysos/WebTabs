using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace WebTabsOpener {
	class Program {
		const string programName = "Web Tabs Opener";
		const string tabsFilesExtension = ".tabs";
		const string tabsFileDocumentClass = "WebTabsCollection";

		/// <summary>File containing addresses to be oppened as browser's tabs.</summary>
		static string tabsFile;
		static TabsOppenerSettings settings = new TabsOppenerSettings();

		static void Main(string[] args) {
			Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
			loadConfig();
			//FileTypeToProgramAssociator.remove(tabsFilesExtension);
			checkFileAssociation();

			if(args.Length == 0) {
				WriteLine("Spefiy path to file with list of paths to be oppened.");
				return;
			}
			tabsFile = args[0];
			if (!File.Exists(tabsFile)) {
				WriteLine($@"The file does not exist ""{tabsFile}""");
				return;
			}
			var warnings = new List<string>();

			var lnks = File.ReadAllLines(tabsFile);

			var sb = new StringBuilder();
			sb.Append(" /new-window ");
			var tabs = new List<TabInfo>();
			foreach (var l in lnks) {
				var tab = parseTabLine(l.Trim());
				tabs.Add(tab);
				if(!linkIsValid(tab)) continue;
				sb.Append($@"""{tab.url}"" ");
			}
			if(!userApproves(tabs)) return;


			var arguments = sb.ToString();
			Process.Start(new ProcessStartInfo() { 
				FileName = settings.targetProgram,
				Arguments = arguments,
				UseShellExecute = true,
			});

			if (warnings.Count == 0) return;
			ForegroundColor = ConsoleColor.Yellow;
			WriteLine("There were some problems with oppening the tabs:");
			foreach (var w in warnings) {
				WriteLine(w);
			}
			ReadKey();
		}

		private static void loadConfig() {
			WriteLine("Loading config file...");
			settings.load();
			if (settings.issues.Count == 0) return;
			var lc = ForegroundColor;
			ForegroundColor = ConsoleColor.Red;
			WriteLine("Program encountered some issues when loading configuration:");
			foreach (var i in settings.issues) {
				WriteLine("	" + i);
			}
			ForegroundColor = lc;
		}

		private static void checkFileAssociation() {
			var p = FileTypeToProgramAssociator.getDefaultProgram(tabsFilesExtension);
			bool pathIsCurrent = false;
			var exePath = Process.GetCurrentProcess().MainModule.FileName;
			if (p != null) {
				pathIsCurrent = Path.GetFullPath(p).Equals(exePath);
			}
			if (pathIsCurrent) return;
			WriteLine($@"{tabsFilesExtension} files are not associated with {programName}.");
			WriteLine($@"Do you want to associate {tabsFilesExtension} files so that they will be oppened with the program by defualt? [Y/N]");
			if (!getYesOrNo()) return;
			FileTypeToProgramAssociator.associate(
				tabsFilesExtension,
				tabsFileDocumentClass,
				exePath
			);
		}

		private static Regex addressMatch = new Regex(@" \w+:\/\/");
		private static TabInfo parseTabLine(string l) {
			var am = addressMatch.Match(l);
			if (am.Index < 0) return null;
			var t = l.Substring(0, am.Index);
			var a = l.Substring(am.Index).Trim();
			return new TabInfo() { title = t, url = a };
		}

		private static bool userApproves(IReadOnlyList<TabInfo> tabs) {
			if (!settings.preview) return true;
			WriteLine($@"The program will try to open following tabs with a ""{settings.targetProgram}"":");
			var pc = Console.ForegroundColor;
			foreach (var l in tabs) {
				ForegroundColor = ConsoleColor.Yellow;
				Write(l.title);
				ForegroundColor = ConsoleColor.Blue;
				WriteLine(" " + l.url);
			}
			ForegroundColor = pc;
			WriteLine("Do you want to proceed?[Y/N]");
			return getYesOrNo();
		}

		private static bool linkIsValid(TabInfo tab) {
			if (tab == null) return false;
			return true;
		}

		private static bool getYesOrNo() {
			string r = null;
			while (r == null) {
				r = ReadLine();
				if (r == "y" || r == "Y") return true;
				if (r == "n" || r == "N") return false;
				r = null;
			}
			return false;
		}
	}

	public class TabInfo {
		public string title { get; set; }
		public string url { get; set; }
	}
}
