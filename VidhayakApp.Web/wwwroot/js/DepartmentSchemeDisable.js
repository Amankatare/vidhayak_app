
//Disable and unable department field in complaint form

function handleComplaintSubCategoryChange() {
    var subCategoryInput = document.getElementById('subCategoryComplaintInput');
    var departmentInput = document.getElementById('departmentInput');
    var descriptionInput = document.getElementById('descriptionId');
    var titleInput = document.getElementById('titleId');
    var submitInput = document.getElementById('submitId');

    // Check if "Related to Personal" is selected
    if (subCategoryInput.value === "Personal") {

        // Disable the Department dropdown and other inputs
        departmentInput.disabled = true;
        descriptionInput.disabled = true;
        titleInput.disabled = true;
        submitInput.disabled = true;

        departmentInput.value = '';
        descriptionInput.value = '';
        titleInput.value = '';
        submitInput.value = '';
    } else {

        // Enable the Department dropdown and other inputs
        departmentInput.disabled = false;
        descriptionInput.disabled = false;
        titleInput.disabled = false;
        submitInput.disabled = false;
    }
}

function handleDemandSubCategoryChange() {
    var subCategory_Input = document.getElementById('subCategoryDemandInput');
    var schemeid = document.getElementById('SchemeInput');
    var dpinput = document.getElementById('dpId');
    var tinput = document.getElementById('tId');
    var submitdemandinput = document.getElementById('submitDemandId');

    // check if "related to personal" is selected
    if (subCategory_Input.value === "Personal") {

        // disable the scheme dropdown and other inputs
        schemeid.disabled = true;
        dpinput.disabled = true;
        tinput.disabled = true;
        submitdemandinput.disabled = true;

        schemeid.value = '';
        dpinput.value = '';
        tinput.value = '';
        submitdemandinput.value = '';
    } else {
        // enable the scheme dropdown and other inputs
        schemeid.disabled = false;
        dpinput.disabled = false;
        tinput.disabled = false;
        submitdemandinput.disabled = false;
    }
}















