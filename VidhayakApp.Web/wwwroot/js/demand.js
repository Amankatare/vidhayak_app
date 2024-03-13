



//Disable and unable department field in complaint form 

function handleSubCategoryChange() {

    var subCategoryInput = document.getElementById('subCategoryInput');
    var SchemeInput = document.getElementById('departmentInput');
    var descriptionInput = document.getElementById('descriptionId');
    var titleInput = document.getElementById('titleId');
    var submitInput = document.getElementById('submitId');

    // Check if "Related to Personal" is selected
    if (subCategoryInput.value === '4') {
        // Disable the Department dropdown
        departmentInput.disabled = true;
        descriptionInput.disabled = true;
        titleInput.disabled = true;
        submitInput.disabled = true;
        // Reset its value
        departmentInput.value = '';
        descriptionInput.value = '';
        titleInput.value = '';
        submitInput.value = '';
    } else {
        // Enable the Department dropdown
        departmentInput.disabled = false;
        descriptionInput.disabled = false;
        titleInput.disabled = false;
        submitInput.disabled = false;
    }

}

