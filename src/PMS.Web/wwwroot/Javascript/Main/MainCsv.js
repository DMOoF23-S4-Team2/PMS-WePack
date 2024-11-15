const csvNav = document.getElementById("csv-nav");
const heroEl = document.getElementById("hero-container");


import { uploadCsv } from "../Csv/UploadCsv.js";

if(csvNav) {
    csvNav.addEventListener("click", () => {
        // Hide other buttons in the interface
        document.querySelector(".add-category-btn").style.display = "none";
        document.querySelector(".add-product-btn").style.display = "none";
    
        // Set the HTML structure in heroEl
        heroEl.innerHTML = `
            <div class="csv-section">
                <div class="csv-actions-container">
                    <div class="csv-btn-container">
                        <button class="update-btn disabled" id="update-shopify-btn">Update Shopify<i class="fa-brands fa-shopify"></i></button>
                        <button disabled class="update-btn disabled">Update Magento<i class="fa-brands fa-magento"></i></button>
                    </div>
                    <div class="csv-btn-container">
                        <button id="add-btn" class="succes-btn disabled">Add</button>
                        <button id="delete-btn" class="yes-delete-btn disabled">Delete</button>  
                    </div>                              
                </div>
                <div class="file-container">
                    <i class="fa-solid fa-cloud-arrow-up"></i>
                    <input type="file" id="csv-upload" accept=".csv" style="display: none;" />
                    <div class="path-input-container">
                        <label for="file-path">Enter file path:</label>
                        <input type="text" id="file-path" placeholder="e.g. C:\\Users\\YourName\\Documents\\file.csv" />
                    </div>
                    <div class="progress-bar-container" style="display: none;">
                        <progress id="file-progress" value="0" max="100"></progress>
                        <p id="progress-text">0%</p>
                    </div>
                    <div id="upload-complete" style="display: none;">
                        <p>File uploaded successfully!<span><i class="fa-solid fa-circle-check"></i></span></p>             
                        <span id="file-name"></span>
                        <button id="clear-btn" class="yes-delete-btn">Clear</button>
                    </div>
                </div>
            </div>
        `;
    
        // Reference new elements
        const filePathInput = document.getElementById("file-path");
        const addButton = document.getElementById("add-btn");
        const updateButton = document.getElementById("update-shopify-btn");
        const deleteButton = document.getElementById("delete-btn");
    
    
         // Enable/disable buttons based on keywords in the file path
         filePathInput.addEventListener("input", () => {
            const filePathValue = filePathInput.value.toLowerCase();
    
            // Check for keywords in the file path and update button states
            if (filePathValue.includes("create")) {
                enableButton(addButton)
                disableButton(updateButton, deleteButton)
    
            } else if (filePathValue.includes("update")) {
                enableButton(updateButton)
                disableButton(addButton, deleteButton)           
    
            } else if (filePathValue.includes("delete")) {
                enableButton(deleteButton)
                disableButton(addButton, updateButton)
    
            } 
        });
    
        addButton.addEventListener("click", () => handleUpload(filePathInput.value));
    
        updateButton.addEventListener("click", () => {
            showModal("Are you sure you want to update the CSV?", () => handleUpload(filePathInput.value));
        });
    
        deleteButton.addEventListener("click", () => {
            showModal("Are you sure you want to delete the CSV?", () => handleUpload(filePathInput.value));
        });
    });

}




// Helper function to handle the upload
export async function handleUpload(filePath) {
    if (!filePath) {
        alert("Please enter a file path.");
        return;
    }

    try {
        // Call uploadCsv with the entered file path
        if (filePath.includes(`"`)) {
            filePath = filePath.replace(/"/g, "");  
        }

        await uploadCsv(filePath);
        document.getElementById("file-path").value = ""; // clear input after upload
    } catch (error) {
        console.error("Failed to upload file:", error);
    }
}

// Show a modal with a custom message and confirmation actions
export function showModal(message, onConfirm) {
    const modal = document.createElement("dialog");
    modal.classList.add("delete-dialog");
    modal.innerHTML = `
        <div class="modal-content">
            <i class="fa-solid fa-triangle-exclamation"></i>
            <p>${message}</p>
            <button class="yes-delete-btn" id="confirm-btn">Confirm</button>
            <button class="no-delete-btn" id="cancel-btn">Cancel</button>
        </div>
    `;
    document.body.appendChild(modal);

    modal.showModal();

    // Handle confirm and cancel
    document.getElementById("confirm-btn").addEventListener("click", () => {
        modal.close();
        document.body.removeChild(modal);
        onConfirm(); 
    });

    document.getElementById("cancel-btn").addEventListener("click", () => {
        modal.close();
        document.body.removeChild(modal);
    });
}



function disableButton(...buttons) {
    buttons.forEach(button => {
        button.disabled = true;
        button.classList.add("disabled");
    }); 
}

function enableButton(button) {
        button.disabled = false;
        button.classList.remove("disabled");
    } 



       
