document.addEventListener('DOMContentLoaded', function () {
    // Select the toggle button
    const sidebarToggle = document.querySelector('[data-bs-toggle="collapse"]');

    // Select the sidebar itself
    const sidebar = document.querySelector('.sidebar');

    const currentPath = window.location.pathname;

    const navLinks = document.querySelectorAll('.nav-link');

    // Add event listener to the toggle button
    sidebarToggle.addEventListener('click', function () {
        // Toggle the 'show' class on the sidebar to open/close it
        sidebar.classList.toggle('show');
    });

    navLinks.forEach(function (link) {
        // Check if the link's href matches the current path
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active-link');
        }
    });
});

