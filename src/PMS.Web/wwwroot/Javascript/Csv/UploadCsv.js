import { showMessage } from "../../Components/MessageBox.js";

export async function uploadCsv(filePath) {  // Accept filePath as a parameter
    try {

        const response = await fetch(`https://localhost:7225/api/Csv/upload-csv?filepath=${filePath}`, {
            method: "POST",
            headers: {
                "Content-Type": "text/plain"  // or "text/json" if you change the API to expect a JSON body
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
    if(filepath.includes("create")) {
        showMessage("Csv added successfully!", true);
    }
    else if(filepath.includes("delete")) {
        showMessage("Csv deleted successfully!", true);
    }
    else if(filepath.includes("update")) {
        showMessage("Csv updated successfully!", true);
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
