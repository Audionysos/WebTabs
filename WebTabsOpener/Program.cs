using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace WebTabsOpener {
	class Program {
		static string tabsFile;
		static bool preview = true;
		static string targetProgram = "chrome.exe";

		static void Main(string[] args) {
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
			if(!userApproves(lnks)) return;

			var sb = new StringBuilder();
			foreach (var l in lnks) {
				var tab = parseTabLine(l.Trim());
				if(!linkIsValid(tab)) continue;
				sb.Append($@"""{tab.url}"" ");
			}
			var arguments = sb.ToString();
			Process.Start(new ProcessStartInfo() { 
				FileName = targetProgram,
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

		private static Regex addressMatch = new Regex(@" \w+:\/\/");
		private static TabInfo parseTabLine(string l) {
			var am = addressMatch.Match(l);
			if (am.Index < 0) return null;
			var t = l.Substring(0, am.Index);
			var a = l.Substring(am.Index);
			return new TabInfo() { title = t, url = a };
		}

		private static bool userApproves(string[] lnks) {
			if (!preview) return true;
			WriteLine($@"The program will try to open following tabs with a ""{targetProgram}"":");
			foreach (var l in lnks) {
				WriteLine(l);
			}
			WriteLine("Do you want to proceed?[Y/N]");
			string r = null;
			while(r == null) {
				r = ReadLine();
				if (r == "y" || r == "Y") return true;
				if (r == "n" || r == "N") return false;
			}
			return false;
		}

		private static bool linkIsValid(TabInfo tab) {
			if (tab == null) return false;
			return true;
		}
	}

	public class TabInfo {
		public string title { get; set; }
		public string url { get; set; }
	}
}
