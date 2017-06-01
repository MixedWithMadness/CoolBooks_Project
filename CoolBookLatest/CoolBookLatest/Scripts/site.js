
// Slide Animation code start

$(window).scroll(function () {
    $(".slideanim").each(function () {
        var pos = $(this).offset().top;

        var winTop = $(window).scrollTop();
        if (pos < winTop + 600) {
            $(this).addClass("slidex");
        }
    });
});

// Slide Animation code end


// Navbar shrink code start

$(window).scroll(function () {
    if ($(document).scrollTop() > 50) {
        $('nav').addClass('shrink');
    } else {
        $('nav').removeClass('shrink');
    }
});

$(function () {
    $("#questionInput").keyup(function () {
        var charsLeft = $(this).attr("maxlength") - $(this).val().length;
        $("#charsLeft").text(charsLeft + " characters left");
    });
});

// Navbar shrink code end