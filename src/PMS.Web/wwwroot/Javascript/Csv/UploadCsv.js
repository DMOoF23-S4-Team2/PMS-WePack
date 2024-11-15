import { showMessage } from "../../Components/MessageBox.js";

export async function uploadCsv(filePath) {  
    try {

        const response = await fetch(`https://localhost:7225/api/Csv/upload-csv?filepath=${filePath}`, {
            method: "POST",
            headers: {
                "Content-Type": "text/plain"  
            }, 
            body: filePath
        });

        console.log(filePath)

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const textResponse = await response.text(); 

        showSuccessMessage(filePath)

        return textResponse;
    } catch (error) {
        console.error("Failed to upload Csv:", error.message);

        showErrorMessage(filePath)
    }
}


function showSuccessMessage(filepath) {
    // Define the regex pattern for a valid .csv file path
    const filePathPattern = /^[a-zA-Z]:\\(?:[^\\/:*?"<>|]+\\)*[^\\/:*?"<>|]+\.csv$/;

    
    if (filePathPattern.test(filepath)) {
        
        if (filepath.toLowerCase().includes("create")) {
            showMessage("CSV added successfully!", true);
        } else if (filepath.toLowerCase().includes("delete")) {
            showMessage("CSV deleted successfully!", true);
        } else if (filepath.toLowerCase().includes("update")) {
            showMessage("CSV updated successfully!", true);
        }
    } else {        
        showMessage("Invalid file path", false);
    }
}


function showErrorMessage(filepath) {
    if(filepath.includes("create")) {
        showMessage("Failed to add Csv!", false);
    }
    else if(filepath.includes("delete")) {
        showMessage("Failed to delete Csv!", false);
    }
    else if(filepath.includes("update")) {
        showMessage("Failed to update Csv!", false);
    }

}
