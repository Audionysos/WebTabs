using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebTabsOpener {
	public class FileTypeToProgramAssociator {

		public FileTypeToProgramAssociator() {

		}

		public static string getDefaultProgram(string extension) {
			var eK = Registry.ClassesRoot.OpenSubKey(extension);
			if (eK == null) return null;
			var cN = eK.GetValue("") as string;
			if (cN == null) return null;
			var cK = Registry.ClassesRoot.OpenSubKey(cN);
			if (cK == null) return null;
			var cmK = cK.OpenSubKey(@"shell\open\command");
			if (cmK == null) return null;
			var cV = cmK.GetValue("") as string;
			if (cV == null) return null;
			return extractProgramPathFromShellCommand(cV);
		}

		private static string extractProgramPathFromShellCommand(string c) {
			if (c[0] != '"') return null;
			var ei = c.IndexOf('"', 1);
			var p = c.Substring(1, ei - 1);
			return p;
		}

		public static void Associate(string extension, string documentClass, string targetProgramPath) {
			var e = extension;
			var dc = documentClass;
			var pp = targetProgramPath;

			var eK = Registry.ClassesRoot.OpenSubKey(e);
			if (eK == null)
				eK = Registry.ClassesRoot.CreateSubKey(e);
			var eV = eK.GetValue("") as string;
			if (eV == null) {
				eK.SetValue("", documentClass);
				eV = documentClass;
			}
			//TODO: Change class?

			var commandSubkey = @"shell\open\command";
			var cK = Registry.ClassesRoot.OpenSubKey(eV);
			if (cK == null)
				cK = Registry.ClassesRoot.CreateSubKey(eV);
			var cmK = cK.OpenSubKey(commandSubkey);
			if (cmK == null)
				cmK = cK.CreateSubKey(commandSubkey);
			var cmV = cmK.GetValue("");
			if(cmV == null) {
				cmK.SetValue("", $@"""[{pp}]"" ""%1""");
			}else {
				//TODO: Confirm override
			}
		}

	}
}
