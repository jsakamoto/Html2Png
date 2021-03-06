﻿// http://www.nilab.info/z3/20110925_02.html
// [@nilab](https://twitter.com/nilab)

var page = new WebPage(),
  address, output, size;

if (phantom.args.length != 4) {
    console.log('Usage: rasterize.js width height URL filename');
    phantom.exit();
} else {
    address = phantom.args[2];
    output = phantom.args[3];
    page.viewportSize = { width: phantom.args[0], height: phantom.args[1] };
    page.open(address, function (status) {
        if (status !== 'success') {
            console.log('Unable to load the address!');
        } else {
            window.setTimeout(function () {
                page.render(output);
                phantom.exit();
            }, 1000);
        }
    });
}