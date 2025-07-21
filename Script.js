// --- JavaScript for Frontend Logic ---

// IMPORTANT: Update this URL if your ASP.NET Core API runs on a different port.
// The default development port is usually 5000 or 5001 (HTTPS).
// You can find the exact URL when you run your ASP.NET Core project in the terminal.
// Make sure this matches the URL your backend API is listening on (e.g., http://localhost:5192)
const API_BASE_URL = 'http://localhost:5192/api/'; 

// Ensure all DOM element references and event listeners are set up after the DOM is fully loaded.
document.addEventListener('DOMContentLoaded', () => {
    // Get references to DOM elements
    const navButtons = document.querySelectorAll('.btn-nav');
    const formSections = document.querySelectorAll('.form-section');
    const messageBox = document.getElementById('messageBox');

    // Employee form specific elements for address checkbox
    const employeePresentAddress = document.getElementById('employeePresentAddress');
    const employeePermanentAddress = document.getElementById('employeePermanentAddress');
    const sameAsPresentAddressCheckbox = document.getElementById('sameAsPresentAddress');

    // Event listeners for navigation buttons to switch forms
    navButtons.forEach(button => {
        button.addEventListener('click', () => {
            // Remove 'active' class from all buttons and form sections
            navButtons.forEach(btn => btn.classList.remove('active'));
            formSections.forEach(section => section.classList.remove('active'));
            // Ensure messageBox is not null before accessing its classList
            if (messageBox) {
                messageBox.classList.add('hidden'); // Hide messages when switching forms
            }

            // Add 'active' class to the clicked button and corresponding form section
            button.classList.add('active');
            const targetId = button.id.replace('nav', '').toLowerCase() + 'FormSection';
            const targetSection = document.getElementById(targetId);
            if (targetSection) {
                targetSection.classList.add('active');
            }
        });
    });

    /**
     * Displays a message in the message box.
     * @param {string} message - The message to display.
     * @param {boolean} isSuccess - True for success message (green), false for error (red).
     */
    function showMessage(message, isSuccess) {
        if (!messageBox) {
            console.error('Message box element not found.');
            return;
        }
        messageBox.textContent = message;
        messageBox.classList.remove('hidden', 'success', 'error');
        if (isSuccess) {
            messageBox.classList.add('success');
        } else {
            messageBox.classList.add('error');
        }
        // Automatically hide the message after 5 seconds
        setTimeout(() => {
            messageBox.classList.add('hidden');
        }, 5000);
    }

    /**
     * Handles form submission for all forms.
     * @param {Event} event - The form submission event.
     * @param {string} endpoint - The API endpoint to send data to (e.g., 'Employees', 'Customers').
     */
    async function handleSubmit(event, endpoint) {
        event.preventDefault(); // Prevent default form submission (page reload)

        const form = event.target;
        const formData = new FormData(form);
        const data = {};

        // Convert FormData to a plain JavaScript object
        for (let [key, value] of formData.entries()) {
            // Handle number conversion for ProductID and Quantity
            if (key === 'productID' || key === 'quantity') {
                data[key] = parseFloat(value); // Use parseFloat for decimal quantities
            } else {
                data[key] = value;
            }
        }

        console.log(`Sending data to ${API_BASE_URL}${endpoint}:`, data); // For debugging

        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data) // Send data as JSON string
            });

            if (response.ok) {
                // Request was successful (status 200-299)
                const result = await response.text(); // Read response as text
                showMessage(`Success: ${result || 'Data saved successfully!'}`, true);
                form.reset(); // Clear the form fields on success
                // Reset checkbox and permanent address field after form submission
                if (form.id === 'employeeForm') {
                    sameAsPresentAddressCheckbox.checked = false;
                    employeePermanentAddress.readOnly = false;
                    employeePermanentAddress.value = '';
                }
            } else {
                // Request failed (status 400-599)
                const errorText = await response.text(); // Read error response
                showMessage(`Error: ${response.status} - ${errorText || 'Failed to save data.'}`, false);
                console.error('API Error:', response.status, errorText);
            }
        } catch (error) {
            // Network error or other issues
            showMessage(`Network Error: Could not connect to the API. Is the backend running? (${error.message})`, false);
            console.error('Fetch Error:', error);
        }
    }

    // Attach event listeners to each form
    document.getElementById('employeeForm').addEventListener('submit', (e) => handleSubmit(e, 'Employees'));
    document.getElementById('customerForm').addEventListener('submit', (e) => handleSubmit(e, 'Customers'));
    document.getElementById('productPurchaseForm').addEventListener('submit', (e) => handleSubmit(e, 'ProductPurchases'));
    document.getElementById('productSalesForm').addEventListener('submit', (e) => handleSubmit(e, 'ProductSales'));

    // Logic for "Permanent address same as Present address" checkbox
    if (sameAsPresentAddressCheckbox && employeePresentAddress && employeePermanentAddress) {
        sameAsPresentAddressCheckbox.addEventListener('change', () => {
            if (sameAsPresentAddressCheckbox.checked) {
                employeePermanentAddress.value = employeePresentAddress.value;
                employeePermanentAddress.readOnly = true;
                employeePermanentAddress.classList.add('bg-gray-100'); // Optional: Add a class for visual feedback
            } else {
                employeePermanentAddress.value = '';
                employeePermanentAddress.readOnly = false;
                employeePermanentAddress.classList.remove('bg-gray-100');
            }
        });

        // Also, update permanent address if present address changes while checkbox is checked
        employeePresentAddress.addEventListener('input', () => {
            if (sameAsPresentAddressCheckbox.checked) {
                employeePermanentAddress.value = employeePresentAddress.value;
            }
        });
    }

    // Initial setup: Ensure the first form is active on load
    document.getElementById('navEmployee').click(); // Simulate click to activate Employee form
});
