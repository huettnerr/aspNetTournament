$(document).ready(function () {
    $("#seedsTable tbody").sortable({
        handle: "td:not(.non-draggable)", // Specify the drag handles
        update: function (event, ui) {
            // Get the reordered item IDs and update the server
            var newSeedNrs = $(this).sortable("toArray", { attribute: "seed-nr" });
            $.ajax({
                url: controllerUrl, // Adjust the URL
                method: "POST",
                data: { newSeedNrs: newSeedNrs },
                success: function (response) {
                    if (response.redirect) {
                        // Handle the redirect
                        window.location.href = response.redirect;
                    } else {
                        // Handle other responses if needed
                        console.log('Operation completed.');
                    }
                },
                error: function () {
                    console.error('Error occurred during the AJAX request.');
                }
            });
        }
    });
    $("#seedsTable tbody").disableSelection();
});