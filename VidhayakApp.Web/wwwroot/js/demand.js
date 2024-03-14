
    document.getElementById("categoryInput").addEventListener("change", function() {
        var selectedCategory = this.value;

        // Use the selectedCategory value to determine the further actions in your JavaScript code
        if (selectedCategory === "Demand") {
            // Show the demand form
            document.getElementById("demandForm").style.display = "block";
            document.getElementById("complaintForm").style.display = "none";
        } else if (selectedCategory === "Complaint") {
          
            // Show the complaint form
            document.getElementById("complaintForm").style.display = "block";
            document.getElementById("demandForm").style.display = "none";
        } else {
            // Hide both forms or do nothing
            document.getElementById("demandForm").style.display = "none";
            document.getElementById("complaintForm").style.display = "none";
        }
    });

