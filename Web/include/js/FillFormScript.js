function checkResponses() {
    $(".responses").each(function () {
        var result = checkResponsesHelper(this);
        console.log(result);
    });
};

function checkResponsesHelper(that) {
    var radioCount = $(that).find("input:radio").length;
    var randomRadio = Math.floor((Math.random() * radioCount));

    var selectedRadio = $(that).find("input:radio:eq(" + randomRadio + ")");
    selectedRadio.prop("checked", true).click();

    var nextTr = $(that).closest("tr").next("tr");
    var errorMessage = nextTr.find(".error").children("div");
    if (errorMessage.text() !== undefined && errorMessage.text().trim() !== "") {
        selectedRadio.prop("checked", false);
        nextTr.addClass("inactive");
        errorMessage.remove();
        checkResponsesHelper(that);

        return false;
    } else {
        return true;
    }
}