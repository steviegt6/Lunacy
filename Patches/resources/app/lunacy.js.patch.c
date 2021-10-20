const { clipboard } = require("electron");

/* LUNACY IS A LUNAR CLIENT MOD */

let texturesEnabled = false;

const lunacyVersion = "0.1.0";

exports.debugClip = function(text) {
    console.log("Coping text to clipboard: " + text);
    clipboard.writeText(text);
}

exports.areTexturesEnabled = function() {
    return texturesEnabled;
}

exports.createVersionCard = function(ke) {
    class LunacyVersionCard extends ke.Component {
        render() {
          return ke.createElement(
            "div",
            { className: "card" },
            ke.createElement(
              "h1",
              { className: "lunar-text" },
              ke.createElement("i", { className: "fas fa-code-branch mr-1" }),
              "LUNACY VERSION"
            ),
            ke.createElement("h4", null, "v", lunacyVersion),
            ke.createElement(
              "p",
              null,
              "A cool little mod for Lunar's Launcher."
            ),
            ke.createElement(
                "p",
                null,
                "Made for fun."
              )
          );
        }
      }
    
      return LunacyVersionCard;
}