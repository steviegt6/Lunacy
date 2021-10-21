const {
    clipboard
} = require("electron");

/* LUNACY IS A LUNAR CLIENT MOD */

let texturesEnabled = false;

const lunacyVersion = "0.1.0";

exports.debugClip = function(text) {
    console.log("Coping text to clipboard: " + text);
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

exports.getNewBlogPosts = function(blogPosts) {
    blogPosts.pop();
    blogPosts.unshift(getReservedBlogPost());
    return blogPosts;
};

function getReservedBlogPost() {
    let blogPost = new Object();

    blogPost.title = "Lunacy Release";
    blogPost.author = "Uranometry";
    blogPost.image =
        "https://cdn.discordapp.com/attachments/660646817612169247/900533605292445806/yyyyy.png";
    blogPost.excerpt =
        "Lunacy has released as a sort of alpha/beta! New features are coming, stay tuned. <3";
    blogPost.link = "https://www.github.com/Steviegt6/Lunacy/";

    return blogPost;
}