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

    messageContainer.style.background = isSuccess? 'green' : '#F44336';

    // Append the icon and message text to the message container
    messageContainer.appendChild(icon);
    messageContainer.appendChild(messageElement);  // Append the message text


    document.getElementById('hero-section').appendChild(messageContainer)

    setTimeout(() => {
        messageContainer.remove()
    }, 5000)   
}