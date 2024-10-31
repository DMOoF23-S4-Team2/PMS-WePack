const csvNav = document.getElementById("csv-nav");
const heroEl = document.getElementById("hero-container");

import { uploadCsv } from "../Csv/UploadCsv.js";

csvNav.addEventListener("click", () => {
    // Hide other buttons in the interface
    document.querySelector(".add-category-btn").style.display = "none";
    document.querySelector(".add-product-btn").style.display = "none";

    // Set the HTML structure in heroEl
    heroEl.innerHTML = `
        <div class="csv-section">
            <div class="csv-actions-container">
                <div class="csv-btn-container">
                    <button class="update-btn">Update Shopify<i class="fa-brands fa-shopify"></i></button>
                    <button class="update-btn">Update Magento<i class="fa-brands fa-magento"></i></button>
                </div>
                <div class="csv-btn-container">
                    <button id="add-btn" class="succes-btn">Add</button>
                    <button class="yes-delete-btn">Delete</button>  
                </div>                              
            </div>
            <div class="file-container">
                <i class="fa-solid fa-cloud-arrow-up"></i>
                <p>Browse file to upload</p>
                <input type="file" id="csv-upload" accept=".csv" style="display: none;" />
                <div class="path-input-container">
                    <label for="file-path">Enter file path:</label>
                    <input type="text" id="file-path" placeholder="e.g., C:\\Users\\YourName\\Documents\\file.csv" />
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
    const fileInput = document.getElementById("csv-upload");
    const addButton = document.getElementById("add-btn");

    // // Make the file-container clickable to open the file input
    // document.querySelector(".file-container").addEventListener("click", () => {
    //     fileInput.click();
    // });

        addButton.addEventListener("click", async () => {

        const filePath = document.getElementById("file-path").value;
        if (filePath) {
            try {
                // Perform the API call with the manually entered file path
                await uploadCsv(filePath);
            } catch (error) {
                console.error("Failed to upload file:", error);
            }
        } else {
            alert("Please enter a file path.");
        }
    });
});
