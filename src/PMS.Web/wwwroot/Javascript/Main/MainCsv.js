const csvNav = document.getElementById("csv-nav");
const heroEl = document.getElementById("hero-container");


csvNav.addEventListener('click', () => {
    document.querySelector(".add-category-btn").style.display = 'none';
    document.querySelector(".add-product-btn").style.display = 'none';

    heroEl.innerHTML = `
    <div class="csv-section">
        <div class="csv-actions-container">
            <div class="csv-btn-container">
                <button class="update-btn">Update Shopify <i class="fa-brands fa-shopify"></i></button>
                <button class="update-btn">Update Magento <i class="fa-brands fa-magento"></i></button>
            </div>
            <div class="csv-btn-container">
                <button class="succes-btn">Add</button>
                <button class="yes-delete-btn">Delete</button>  
            </div>                              
        </div>
        <div class="file-container">
            <i class="fa-solid fa-cloud-arrow-up"></i>
            <p>Browse file to upload</p>
            <input type="file" id="csv-upload" accept=".png" style="display: none;" />
            <div class="progress-bar-container">
                <progress id="file-progress" value="0" max="100"></progress>
                <span id="progress-text">0%</span>
            </div>
            <div id="upload-complete">
                <p>File uploaded successfully!   <span><i class="fa-solid fa-circle-check"></i></span></p>             
                <span id="file-name"></span>
                <button id="clear-btn" class="yes-delete-btn">Clear</button>
            </div>
        </div>
    </div>
    `;

    // Attach the event listener AFTER the new DOM has been added
    const fileInput = document.getElementById('csv-upload');
    const progressBar = document.getElementById('file-progress');
    const progressBarContainer = document.querySelector('.progress-bar-container');
    const progressText = document.getElementById('progress-text');
    const uploadCompleteDiv = document.getElementById('upload-complete');
    const fileNameSpan = document.getElementById('file-name');
    const clearButton = document.getElementById('clear-btn');

    // Make the file-container clickable to open the file input
    document.querySelector('.file-container').addEventListener('click', () => {
        fileInput.click();
    });

    // Handle file selection and simulate loading progress
    fileInput.addEventListener('change', function(event) {
        const file = event.target.files[0];
        if (file) {
            console.log(`Selected file: ${file.name}`);

            // Show the progress bar container when a file is selected
            progressBarContainer.style.display = 'block';

            // Simulate the progress
            let progress = 0;
            const progressInterval = setInterval(() => {
                progress += 10; // Increment progress
                progressBar.value = progress;
                progressText.textContent = `${progress}%`; // Update percentage display

                if (progress >= 100) {
                    clearInterval(progressInterval);
                    console.log('File loaded successfully.');



                    // Show the "upload complete" message with a checkmark
                    uploadCompleteDiv.style.display = 'block';

                    // Display the name of the uploaded file
                    fileNameSpan.textContent = `File: ${file.name}`;

                    // Hide progress bar
                    progressBarContainer.style.display = 'none';
                }
            }, 200); // Update every 200ms (adjust timing as needed)
        }
    });

    // Handle clearing the upload and resetting the view
    clearButton.addEventListener('click', () => {
        // Hide upload complete message and reset everything
        uploadCompleteDiv.style.display = 'none';
        fileInput.value = ''; // Reset the file input
        progressBar.value = 0; // Reset progress bar
        progressText.textContent = '0%'; // Reset progress text
        fileNameSpan.textContent = ''; // Clear the file name
    });
});
