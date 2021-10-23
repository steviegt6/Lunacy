const {
    clipboard
} = require("electron");

/* LUNACY IS A LUNAR CLIENT LAUNCHER MOD */

let texturesEnabled = false;

const lunacyVersion = "0.1.0";

const blogUrl = "https://tomat.dev/assets/lunacy/blog.json";

exports.debugClip = function(text) {
    console.log("Copying text to clipboard: " + text);
    clipboard.writeText(text);
};

exports.areTexturesEnabled = function() {
    return texturesEnabled;
};

exports.createVersionCard = function(ke) {
    class LunacyVersionCard extends ke.Component {
        render() {
            return ke.createElement(
                "div", {
                    className: "card"
                },
                ke.createElement(
                    "h1", {
                        className: "lunar-text"
                    },
                    ke.createElement("i", {
                        className: "fas fa-code-branch mr-1"
                    }),
                    "LUNACY VERSION"
                ),
                ke.createElement("h4", null, "v", lunacyVersion),
                ke.createElement("p", null, "A cool little mod for Lunar's Launcher."),
                ke.createElement("p", null, "Made for fun.")
            );
        }
    }

    return LunacyVersionCard;
};

exports.getSettingsPage = function(ke) {
    class LunacySettingsPage extends ke.Component {
        render() {
            return ke.createElement(
                "div",
                { className: "large-container container-fluid" },
                ke.createElement(
                    "div",
                    { className: "row" },
                    ke.createElement(
                        "div",
                        { id: "client-settings", className: "col-12" },
                        ke.createElement(
                            "div",
                            { className: "card" },
                            ke.createElement(
                                "div",
                                { id: "client-settings-title", className: "large-card-title lunar-text" },
                                ke.createElement(
                                    "hi",
                                    null,
                                    ke.createElement("i", { className: "fas fa-cogs" }),
                                    "Lunacy Settings"
                                ),
                                ke.createElement(
                                    "h6",
                                    null,
                                    "Changes & Tweaks for Lunacy"
                                )
                            ),
                            ke.createElement(
                                "div",
                                { id: "client-settings-content", className: "card-body"},
                                ke.createElement(
                                    "div",
                                    null,
                                    ke.createElement(
                                        "div", 
                                        { className: "col-8" },
                                        ke.createElement(
                                            "div",
                                            { className: "col-6 mb-3" },
                                            ke.createElement(
                                                AfterLaunchActionSetting_AfterLaunchActionSetting,
                                                null
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }
    }

    return LunacySettingsPage;
}

exports.getNewBlogPosts = function(blogPosts) {
    blogPosts.pop();
    blogPosts.unshift(getReservedBlogPost());
    return blogPosts;
};

function getReservedBlogPost() {
    var req = new XMLHttpRequest();
    req.open("GET", blogUrl, false);
    req.send(null);

    return JSON.parse(req.responseText);
}

function createTestSettingComponent(ke, self) {
    class TestSettignComponent extends ke.Component {

    }
}
