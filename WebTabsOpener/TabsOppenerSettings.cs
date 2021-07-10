using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#nullable enable

namespace WebTabsOpener {
	public class TabsOppenerSettings {

		/// <summary>Path to configuraton file from where the setings are loaded.</summary>
		public string configFile = "config";
		/// <summary>Path to browser program in which the tabs will be oppened.</summary>
		public string targetProgram { get; set; } = "chrome.exe";
		/// <summary>If true, the program will show all the pages that suppose to be oppened and will wait for confirmation.</summary>
		public bool preview { get; set; } = true;

		private List<string> _issues = new List<string>();
		public IReadOnlyList<string> issues => _issues.AsReadOnly();

		public void load() {
			_issues.Clear();
			if (!File.Exists(configFile)) {
				createConfigFile();
				_issues.Add($@"Config file not foud at ""{Path.GetFullPath(configFile)}"". New config file will be created.");
				return;
			}
			try { 
				var lns = File.ReadAllLines(configFile);
				load(lns);
			}catch (Exception e) {
				_issues.Add($@"Unexcpected error occurend when loading configuration file: {e}");
			}
		}
			

		private void load(string[] config) {
			string cn = ""; //current property name
			string sv = ""; //string value
			var ln = -1; //line number
			for (int i = 0; i < config.Length; i++) {
				var l = config[i];
				if (l.StartsWith("//")) continue;
				var le = l.IndexOf(':');
				if (le == 0) sv += l.Substring(le).Trim(); //multiline of previous value
				else {
					load:
					if (!string.IsNullOrEmpty(cn)) {
						var err = loadProperty(cn, sv);
						if (err != null) 
							_issues.Add($@"Parsing error at line {ln}: {err}");
					}
					ln = i + 1;
					cn = l.Substring(0, le).Trim();
					sv = l.Substring(le+1).Trim();
					if (i == config.Length - 1) { i++; goto load; }
				}
			}
		}

		private string? loadProperty(string cn, string sv) {
			var t = GetType();
			var p = t.GetProperty(cn, BindingFlags.Public | BindingFlags.Instance);
			if (p == null) return $@"Setting has no property named ""{cn}"".";
			try {
				object v = Convert.ChangeType(sv, p.PropertyType);
				p.SetValue(this, v);
			}catch (Exception e) {
				return $@"Could not parse value for a ""{cn}"" property: {e.Message}";
			}
			return null;
		}

		private void createConfigFile() {
			var cc =
			$"//Path to browser program in which the tabs will be oppened.\n" +
			$"targetProgram: {targetProgram}\n" +
			$"//If True, the program will show all the pages that suppose to be oppened and will wait for confirmation.\n" +
			$"preview: {preview}\n";

			File.WriteAllTextAsync(configFile, cc);

		}
	}
}