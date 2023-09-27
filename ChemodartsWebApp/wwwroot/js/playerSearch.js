$(document).ready(function () {
    $("#playerSearch").on("input", function () {
        var searchText = $(this).val().toLowerCase();
        $("#playerList option").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});