using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebTabsOpener {

	//TODO: File association was not tested on blank scenario 
	public class FileTypeToProgramAssociator {

		public FileTypeToProgramAssociator() {

		}

		/// <summary>Returns path to executable which is set as default OS application to open files with given extension or null if there is no progrogram associtated with the file extension.</summary>
		/// <param name="extension">Extension including '.' dont sign. For example ".png".</param>
		/// <returns></returns>
		public static string getDefaultProgram(string extension) {
			var eK = Registry.ClassesRoot.OpenSubKey(extension); //extension key
			if (eK == null) return null;
			var cN = eK.GetValue("") as string; //class name
			if (cN == null) return null;
			var cK = Registry.ClassesRoot.OpenSubKey(cN); //class key
			if (cK == null) return null;
			var cmK = cK.OpenSubKey(@"shell\open\command"); //command key
			if (cmK == null) return null;
			var cV = cmK.GetValue("") as string; //command
			if (cV == null) return null;
			return extractProgramPathFromShellCommand(cV);
		}

		private static string extractProgramPathFromShellCommand(string c) {
			if (c[0] != '"') return null;
			var ei = c.IndexOf('"', 1);
			var p = c.Substring(1, ei - 1);
			return p;
		}

		/// <summary>Associate files with given extension to be opened by default with program specified by <paramref name="targetProgramPath"/>.
		/// This method will modify the OS registry.</summary>
		/// <param name="extension">Extension including '.' dont sign. For example ".png".</param>
		/// <param name="documentClass">Class of document the <paramref name="extension"/> is or will be assigned to.
		/// This could be any name for new type of file but it should be unique to not override behavior for different file types.
		/// In reality on MS Windows, the command which executes the program is actually assigned to the "document class" and not the extension, i.e. many extension could be assigned to the same class of document and all files with one of those extensions will be oppened with the same program.</param>
		/// <param name="targetProgramPath">Path to executable of the application that suppose to open the files.</param>
		public static void associate(string extension, string documentClass, string targetProgramPath) {
			var e = extension;
			var dc = documentClass;
			var pp = targetProgramPath;

			var eK = Registry.ClassesRoot.ensureSubKey(e, true); //extension key
			if(!eK.createNewValue("", documentClass)) {
				//TODO: Approve class change?
				eK.SetValue("", documentClass);
				eK.Close();
			}

			//TODO: Not sure where to write class key
			//https://stackoverflow.com/questions/4608505/c-sharp-file-assocation-access-to-the-registry-key-hkey-classes-root-is-den
			var userClasses = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes", true);
			var cK = userClasses.ensureSubKey(documentClass, true); //class key
			var cmK = cK.ensureSubKey(@"shell\open\command", true); //command key
			var command = $@"""{pp}"" ""%1""";
			if (!cmK.createNewValue("", command)) {
				//TODO: Confirm override?
				cmK.SetValue("", command);
			}
			cmK.Close();
			cK.Close();
		}

		/// <summary>Removes association with default oppening program for a given extension.
		/// This method modifies the OS registry.</summary>
		/// <param name="extension">Extension including '.' dont sign. For example ".png".</param>
		public static void remove(string extension) {
			var eK = Registry.ClassesRoot.OpenSubKey(extension, true);
			if (eK == null) return;
			var cN = eK.GetValue("") as string;
			if (cN == null) return;
			eK.SetValue("", "");
		}

	}

	public static class RegistryExtensions {
		//LOL: I just relized the CreateSubKey does the same thing... :]
		public static RegistryKey ensureSubKey(this RegistryKey p, string name, bool writable = false) {
			var sk = p.OpenSubKey(name, writable);
			if (sk == null) sk = p.CreateSubKey(name, writable);
			return sk;

		}

		/// <summary>Attempts to create new entry with given value.
		/// Returns false if entry with given id already exist and don't have the same <paramref name="value"/>, otherwise true is returned.</summary>
		/// <param name="p"></param>
		/// <param name="eid">Id of new entry.</param>
		/// <param name="value">Value for new entry.</param>
		public static bool createNewValue(this RegistryKey p, string eid, object value) {
			var v = p.GetValue(eid);
			if (v == null) p.SetValue(eid, value);
			else if(v != value) return false;
			return true;
		}
	}

}
