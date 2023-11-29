function ConfirmDelete(uniqueId, IsDeleteClicked) {
    var deleteSpan = "deleteSpan_" + uniqueId
    var confirmDeleteSpan = "confirmDeleteSpan_" + uniqueId
    if (IsDeleteClicked) {
        //console.log("Hello")
        $('#' + deleteSpan).hide()
        $('#' + confirmDeleteSpan).show()
    } else {
        $('#' + deleteSpan).show()
        $('#' + confirmDeleteSpan).hide()
    }
}
$(document).ready(function () {
    $('#studenttable').dataTable()
})