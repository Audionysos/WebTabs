'use strict';

chrome.browserAction.onClicked.addListener(function(tab) {
	var s = "";
	browser.tabs.query({}).then(function(tabs){     
			tabs.forEach(function(tab){
				s += tab.title + " " + tab.url + "\n";
			});

			var blob = new Blob([s], {type: "text/plain"});
			var url = URL.createObjectURL(blob);
			chrome.downloads.download({
				url: url, // The object URL can be used as download URL
				filename: "group.tabs" // Optional
			});

		},
		function(error){ console.log(`Error: ${error}`); }
	);

});

