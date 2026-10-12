$(window).on('load', function () {
    $("#spinner").fadeOut("slow");
});

$("nav li a").click(function (e) {
    if ($(this).attr("href") != '#') {
        $("#spinner").fadeIn("slow");
    }
});

$("body").on("click", "#expandcollaps", function (e) {
    if ($(".modal-dialog").attr("class").includes("expand")) {
        $(this).closest(".modal-dialog").removeClass("expand");
        $(this).removeClass("fa fa-compress");
        $(this).addClass("fa fa-expand");
    }
    else {
        $(this).closest(".modal-dialog").addClass("expand");

        $(this).removeClass("fa fa-expand");
        $(this).addClass("fa fa-compress");
    }
})