// Select all nav-container elements
const navItems = document.querySelectorAll('.nav-container');
    

// Add click event listener to each navbar item
navItems.forEach(item => {
    item.addEventListener('click', () => {
        // Remove 'active' class from all nav items
        navItems.forEach(nav => nav.classList.remove('active'));
        
        // Add 'active' class to the clicked nav item
        item.classList.add('active');
    });
});