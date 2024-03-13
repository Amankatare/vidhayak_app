



//Disable and unable department field in complaint form 

function handleDemandSubCategoryChange() {

    var subCategoryInput = document.getElementById('subInput');
    var SchemeInput = document.getElementById('schemeInput');
    var descriptionInput = document.getElementById('dpId');
    var titleInput = document.getElementById('tId');
    var submitInput = document.getElementById('subId');

    // Check if "Related to Personal" is selected
    if (subCategoryInput.value === '4') {
        // Disable the Department dropdown
        SchemeInput.disabled = true;
        descriptionInput.disabled = true;
        titleInput.disabled = true;
        submitInput.disabled = true;
        // Reset its value
        SchemeInput.value = '';
        descriptionInput.value = '';
        titleInput.value = '';
        submitInput.value = '';
    } else {
        // Enable the Department dropdown
        SchemeInput.disabled = false;
        descriptionInput.disabled = false;
        titleInput.disabled = false;
        submitInput.disabled = false;
    }

}

