

$(document).ready(function () {
    $(document).on("click", ".editBtn", function () {
        var id = $(this).data("id");

        $.get("/Home/Edit/" + id, function (data) {
            $("#editTaskModalBody").html(data);
        });
    });

    $(document).on("submit", "#editTaskForm", function (e) {
        e.preventDefault();

        var id = $(this).find('input[name="Id"]').val();

        $.ajax({
            type: "POST",
            url: $("#editTaskForm").attr("action"),
            data: $(this).serialize(),
            success: function (res) {
                if (res.success) {
                    var row = $("#row-" + res.id);

                    row.find("td:nth-child(1)").text(res.description);
                    row.find("td:nth-child(2)").text(res.category);
                    row.find("td:nth-child(4)").text(res.status);

                    if (res.dueDate) {
                        var due = new Date(res.dueDate + "T00:00:00");
                        var today = new Date();

                        var formatted = due.toLocaleDateString("sr-RS");
                        var dueCell = row.find("td:nth-child(3)");
                        var statusCell = row.find("td:nth-child(4)");

                        dueCell.text(formatted);

                        dueCell.removeClass("bg-warning");
                        statusCell.removeClass("bg-warning");

                        if (due < today && res.statusId === "open") {
                            dueCell.addClass("bg-warning");
                            statusCell.addClass("bg-warning");
                        }
                    }

                    var markCell = row.find("td:nth-child(6)");

                    if (res.showMarkComplete) {
                        markCell.html(
                            '<button type="submit" class="custom-btn btn-sm" name="Id" value="'
                            + res.id + '">Mark completed</button>'
                        );
                    } else {
                        markCell.html("");
                    }

                    $("#editTaskModal").modal("hide");
                } else {
                    alert("Error while saving!");
                }
            }
        });
    });

    $(document).on("click", ".deleteBtn", function () {
        var id = $(this).data("id");

        $.ajax({
            type: "POST",
            url: "/Home/Delete",
            data: { id: id },
            success: function (res) {
                if (res.success) {
                    $("#row-" + res.id).remove();
                } else {
                    alert("Error while deleting task!");
                }
            }
        });
    });
});
