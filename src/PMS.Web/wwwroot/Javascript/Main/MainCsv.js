const csvNav = document.getElementById("csv-nav");
const heroEl = document.getElementById("hero-container");

import { addCsv } from "../Csv/AddCsv.js";

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
    const progressBar = document.getElementById("file-progress");
    const progressBarContainer = document.querySelector(".progress-bar-container");
    const progressText = document.getElementById("progress-text");
    const uploadCompleteDiv = document.getElementById("upload-complete");
    const fileNameSpan = document.getElementById("file-name");
    const clearButton = document.getElementById("clear-btn");

    // Make the file-container clickable to open the file input
    document.querySelector(".file-container").addEventListener("click", () => {
        fileInput.click();
    });

    // Show the selected file name
    fileInput.addEventListener("change", () => {
        const file = fileInput.files[0];
        if (file) {
            fileNameSpan.textContent = `Selected file: ${file.name}`;
            uploadCompleteDiv.style.display = "none"; // Reset upload complete message
        }
    });

    // Handle file upload on "Add" button click
    addButton.addEventListener("click", async () => {
        const file = fileInput.files[0];
        if (file) {
            try {
                // Show progress bar and reset progress
                progressBarContainer.style.display = "block";
                progressBar.value = 0;
                progressText.textContent = "0%";

                // Simulate upload progress
                let progress = 0;
                const progressInterval = setInterval(() => {
                    progress += 20; // Increment progress by 20%
                    progressBar.value = progress;
                    progressText.textContent = `${progress}%`;

                    if (progress >= 100) {
                        clearInterval(progressInterval);
                    }
                }, 200); // Adjust as needed

                // Perform the actual file upload
                await addCsv(file);

                // Hide progress bar and show success message
                progressBarContainer.style.display = "none";
                uploadCompleteDiv.style.display = "block";
                fileNameSpan.textContent = `File: ${file.name}`;
            } catch (error) {
                console.error("Failed to upload file:", error);
            }
        } else {
            alert("Please select a file before clicking Add.");
        }
    });

    // Clear button to reset the view
    clearButton.addEventListener("click", () => {
        // Reset everything
        uploadCompleteDiv.style.display = "none";
        fileInput.value = ""; // Reset the file input
        progressBar.value = 0; // Reset progress bar
        progressText.textContent = "0%"; // Reset progress text
        fileNameSpan.textContent = ""; // Clear the file name
    });
});
