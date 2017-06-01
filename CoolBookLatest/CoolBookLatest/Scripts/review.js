$(document).ready(function () {
    $(".rating-star-block .star").mouseleave(function () {
        $("#" + $(this).parent().attr('id') + " .star").each(function () {
            $(this).addClass("outline");
            $(this).removeClass("filled");
        });
    });
    $(".rating-star-block .star").mouseenter(function () {
        var hoverVal = $(this).attr('rating');
        $(this).prevUntil().addClass("filled");
        $(this).addClass("filled");
        $("#RAT").html(hoverVal);
    });
    $(".rating-star-block .star").click(function () {

        var v = $(this).attr('rating');
        var texte = $("#CreateForm input[name='Text']").val();
        var bookid = $("#CreateForm select[name='BookId']").val();
        var titlee = $("#CreateForm input[name='Title']").val();
        var token = $("#CreateForm input[name='__RequestVerificationToken']").val();
        var newScore = 0;
        var updateP = "#" + $(this).parent().attr('id') + ' .CurrentScore';

        $("#" + $(this).parent().attr('id') + " .star").hide();
        $("#myElem").show('1000').delay(2000);
        $("#" + $(this).parent().attr('id') + " .yourScore").html("Your Score is : &nbsp;<b style='color:#ff9900; font-size:15px'>" + v + "</b>");

        $.ajax({
            type: "POST",
            url: "/Reviews/Create/",
            headers: {
                "__RequestVerificationToken": token
            },
            data: "{Title: '" + titlee + "', BookId: '" + bookid + "', Text: '" + texte + "',  Rating: '" + v + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    });
});
function setNewScore(container, data) {
    $(container).html(data);
    $("#myElem").show('1000').delay(2000).queue(function (n) {
        $(this).hide(); n();
    });
}