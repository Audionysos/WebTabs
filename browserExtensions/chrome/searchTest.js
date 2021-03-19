console.log("start");
const jsdom = require("jsdom");
const { JSDOM } = jsdom;
var fs = require('fs');

var x = fs.readFileSync("example_cookie_accept_target.html").toString();

const dom = new JSDOM(x);
const doc = dom.window.document;
const body = doc.body;

var all = body.getElementsByTagName("*");
var mtchs = [];
var types = new Int32Array(10);

for (let i = 0; i < all.length; i++) {
	var e = all[i];
	types[e.nodeType]++;
	if(e.nodeType != 3) continue; 
	console.log(e.textContent);
}

types.forEach(e => {
	console.log(e);
});

