'use strict';

chrome.browserAction.onClicked.addListener(function(tab) {
	var s = "";
	chrome.tabs.getAllInWindow(null,function(tabs){     
		console.log("\n/////////////////////\n");
		tabs.forEach(function(tab){
			//console.log(tab.url);
			//s += "\"" + tab.url + "\n";
			s += tab.title + " " + tab.url + "\n";
		});

		var blob = new Blob([s], {type: "text/plain"});
		var url = URL.createObjectURL(blob);
		chrome.downloads.download({
			url: url, // The object URL can be used as download URL
			filename: "group.tabs" // Optional
		});
	});

});

