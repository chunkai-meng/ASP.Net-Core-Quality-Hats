// Write your Javascript code.
$(document).ready(function () {
    var firstParagraph = $("p:first");
    $("#addContent").on("click", function () {
        $(".body-content").append(firstParagraph.clone());
    });
});