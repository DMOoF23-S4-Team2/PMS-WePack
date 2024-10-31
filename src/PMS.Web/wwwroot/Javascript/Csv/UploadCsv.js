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

        const textResponse = await response.text(); // Get the response as plain text
        console.log("Csv added successfully:", textResponse);

        // Show success message
        showMessage("Csv added successfully!", true);

        return textResponse;
    } catch (error) {
        console.error("Failed to add Csv:", error.message);

        // Show error message
        showMessage(`Failed to add Csv`, false);
    }
}
