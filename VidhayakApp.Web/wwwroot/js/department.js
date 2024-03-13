
//Disable and unable department field in complaint form 

function handleComplaintSubCategoryChange() {

    var subCategoryInput = document.getElementById('subCategoryInput');
    var departmentInput = document.getElementById('departmentInput');
    var descriptionInput = document.getElementById('descriptionId');
    var titleInput = document.getElementById('titleId');
    var submitInput = document.getElementById('submitId');

    // Check if "Related to Personal" is selected
    if (subCategoryInput.value === '4') {
        console.log("inside if of Complaint")
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
        console.log("inside else of Complaint")
        // Enable the Department dropdown
        departmentInput.disabled = false;
        descriptionInput.disabled = false;
        titleInput.disabled = false;
        submitInput.disabled = false;
    }

}

function handleDemandSubCategoryChange() {

    var subInput = document.getElementById('SubInput');
    var schemeId = document.getElementById('schemeInput');
    var dpInput = document.getElementById('dpId');
    var tInput = document.getElementById('tId');
    var submitDemandInput = document.getElementById('subId');

    // Check if "Related to Personal" is selected
    if (subInput.value === '4') {
        console.log("inside if of Demand")
        // Disable the Department dropdown
        schemeId.disabled = true;
        dpInput.disabled = true;
        tInput.disabled = true;
        submitDemandInput.disabled = true;
        // Reset its value
        schemeId.value = '';
        dpInput.value = '';
        tInput.value = '';
        submitDemandInput.value = '';
    } else {
        console.log("inside else of Demand")
        // Enable the Department dropdown
        schemeId.disabled = false;
        dpInput.disabled = false;
        tInput.disabled = false;
        submitDemandInput.disabled = false;
    }

}