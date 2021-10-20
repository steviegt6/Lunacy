const { clipboard } = require("electron");

/* LUNACY IS A LUNAR CLIENT MOD */

let texturesEnabled = false;

exports.debugClip = function(text) {
    console.log("Coping text to clipboard: " + text);
    clipboard.writeText(text);
}

exports.areTexturesEnabled = function() {
    return texturesEnabled;
}