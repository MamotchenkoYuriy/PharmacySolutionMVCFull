function LoudTable() {
    $("#drop-down-list").change(function() {
        $.ajax({
            method: "Get",
            url: "/@Html.ViewContext.RouteData.Values['controller']/GetTablePartialView",
            data: "id=" + $("#drop-down-list").val(),
            success: function(data) {
                $("#table-to-change").html(data);
            },
            error: function() {
                alert("Ошибка установки значения :-(((");
            }
        });
    });
}
