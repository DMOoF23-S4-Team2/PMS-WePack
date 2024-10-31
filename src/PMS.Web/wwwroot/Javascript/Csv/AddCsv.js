import { showMessage } from "../../Components/MessageBox.js";

export async function addCsv(filePath) {  // Accept filePath as a parameter
    try {

        const response = await fetch("https://localhost:7225/api/Csv/upload-csv", {
            method: "POST",
            body: JSON.stringify({filePath})
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const data = await response.json();
        console.log("Csv added successfully:", data);

        // Show success message
        showMessage("Csv added successfully!", true);

        return data;
    } catch (error) {
        console.error("Failed to add Csv:", error.message);

        // Show error message
        showMessage(`Failed to add Csv`, false);
    }
}
