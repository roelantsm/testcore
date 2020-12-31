function confirmDelete(uniqueId, isDeleteClicked) {
    
    console.log("in confirm functie");

    console.log("uniqueId: " + uniqueId);
    console.log("isDeleteClicked: " + isDeleteClicked);
    
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
    
    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
    
}