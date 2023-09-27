$(document).ready(function () {
    $("#playerSearch").on("input", function () {
        var searchText = $(this).val().toLowerCase();
        $("#playerList option").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});

$(document).ready(function () {
    $("#playerSearch").on("input", function () {
        var searchText = $(this).val().toLowerCase();
        $("#playerTable tr:not(:first)").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});

$(document).ready(function () {
    $("#matchesSearch").on("input", function () {
        var searchText = $(this).val().toLowerCase();
        $("#matchesTable tr:not(:first)").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});

