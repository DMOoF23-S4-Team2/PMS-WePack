
export async function addProduct(productData) {
    try {
        const response = await fetch("https://localhost:7225/api/Product", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"  // Specify that we're sending JSON
            },
            body: JSON.stringify(productData)
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const data = await response.json();
        console.log("Product added successfully:", data);

         // Show success message
         showMessage("Product added successfully!", true);

        return data;
    } catch (error) {
        console.error("Failed to add product:", error.message);

        // Show error message
        showMessage(`Failed to add product`, false);
    }
}

export function showMessage(message, isSuccess) {

    const messageContainer = document.createElement('div')
    messageContainer.classList.add('message-container')

    // Create the <p> element for the message text
    const messageElement = document.createElement('p');
    messageElement.textContent = message;  // Set the text of the <p> element

    // Create the <i> element for the icon related to the message
    const icon = document.createElement('i');
    icon.classList.add('fa-solid');  

    icon.classList.add(isSuccess ? 'fa-circle-check' : 'fa-circle-xmark');

    messageContainer.style.background = isSuccess? '#4CAF50' : '#F44336';

    // Append the icon and message text to the message container
    messageContainer.appendChild(icon);
    messageContainer.appendChild(messageElement);  // Append the message text


    document.getElementById('hero-section').appendChild(messageContainer)

    setTimeout(() => {
        messageContainer.remove()
    }, 5000)
    

}
