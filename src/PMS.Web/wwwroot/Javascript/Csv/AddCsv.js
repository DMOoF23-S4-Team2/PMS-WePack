import { showMessage } from "../../Components/MessageBox.js";

export async function addCsv(file) {  // Accept file as a parameter
    try {
        const formData = new FormData();
        formData.append("file", file); // Append the CSV file with a key

        const response = await fetch("https://localhost:7225/api/Csv/add-many-products", {
            method: "POST",
            body: formData  // Use FormData directly as the body
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
