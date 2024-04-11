
//Disable and unable department field in complaint form

function handleComplaintSubCategoryChange() {
    var subCategoryInput = document.getElementById('subCategoryComplaintInput');
    var departmentInput = document.getElementById('departmentInput');
    var descriptionInput = document.getElementById('descriptionId');
    var titleInput = document.getElementById('titleId');
    var submitInput = document.getElementById('submitId');
    var ImageInput = document.getElementById('ImageUploadId');

    // Check if "Related to Personal" is selected
    if (subCategoryInput.value === "Personal") {
        console.log(subCategoryInput.value);
        // Disable the Department dropdown and other inputs
        departmentInput.disabled = true;
        descriptionInput.disabled = true;
        titleInput.disabled = true;
        submitInput.disabled = true;
        ImageInput.disabled = true;

        departmentInput.value = '';
        descriptionInput.value = '';
        titleInput.value = '';
        submitInput.value = '';
        ImageInput.value = '';

    } else if (subCategoryInput.value === "PrivateOrganization")
    {
        console.log(subCategoryInput.value);
        descriptionInput.disabled = false;
        titleInput.disabled = false;
        submitInput.disabled = false;
        // Disable the Department dropdown and other inputs
        departmentInput.disabled = true;
        departmentInput.value = '';
        ImageInput.disabled = true;
        ImageInput.value = '';

    } else if (subCategoryInput.value === "GovernmentDepartments") {
        console.log("no-------");
        // Enable the Department dropdown and other inputs
        departmentInput.disabled = false;
        descriptionInput.disabled = false;
        titleInput.disabled = false;
        submitInput.disabled = false;
        ImageInput.disabled = false;
    } else {
        console.log("no-------");
        // Enable the Department dropdown and other inputs
        departmentInput.disabled = true;
        descriptionInput.disabled = true;
        titleInput.disabled = true;
        submitInput.disabled = true;
        ImageInput.disabled = true;
    }

    console.log(subCategoryInput.value );
    console.log(departmentInput.value );
    console.log(descriptionInput.value );
    console.log(titleInput.value );
    console.log(submitInput.value);
    console.log(ImageInput.value);



}

function handleDemandSubCategoryChange() {
    var subCategory_Input = document.getElementById('subCategoryDemandInput');
    var schemeid = document.getElementById('SchemeInput');
    var dpinput = document.getElementById('dpId');
    var tinput = document.getElementById('tId');
    var submitdemandinput = document.getElementById('submitDemandId');

    // check if "related to personal" is selected
    if (subCategory_Input.value === "Personal") {
        console.log(subCategory_Input.value);
        // disable the scheme dropdown and other inputs
        schemeid.disabled = true;
        dpinput.disabled = true;
        tinput.disabled = true;
        submitdemandinput.disabled = true;

        schemeid.value = '';
        dpinput.value = '';
        tinput.value = '';
        submitdemandinput.value = '';
    } else if (subCategory_Input.value === "PrivateOrganization") {
        console.log(subCategory_Input.value);


        dpinput.disabled = false;
        tinput.disabled = false;
        submitdemandinput.disabled = false;
        // disable the scheme dropdown and other inputs
        schemeid.disabled = true;

    } else if (subCategory_Input.value === "GovernmentSchemes") {

        console.log("no------------");
        // enable the scheme dropdown and other inputs
        schemeid.disabled = false;
        dpinput.disabled = false;
        tinput.disabled = false;
        submitdemandinput.disabled = false;
    } else {
        schemeid.disabled = true;
        dpinput.disabled = true;
        tinput.disabled = true;
        submitdemandinput.disabled = true;
    }
    console.log('subCategory_Input-->',subCategory_Input.value );
    console.log('schemeid.value-->', schemeid.value );
    console.log('dpinput.value-->', dpinput.value );
    console.log('tinput.value-->', tinput.value );
    console.log('submitdemandinput.value-->', submitdemandinput.value );
}















