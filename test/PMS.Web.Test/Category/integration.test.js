//CODE TO TEST DOM INTERACTIONS
/**
 * @jest-environment jsdom
 */


import { describe, it, expect, beforeEach, vi } from 'vitest';

import { addCategory } from '../../../src/PMS.Web/wwwroot/Javascript/Category/AddCategory.js';
import { showMessage } from '../../../src/PMS.Web/wwwroot/Components/MessageBox.js';

// Mocking the fetch function before running tests
beforeEach(() => {
    // Reset fetch mock before each test
    global.fetch = vi.fn();
});

describe('addCategory', () => {
    // Add DOM setup
    beforeEach(() => {
        // Set up the DOM structure that your function relies on
        document.body.innerHTML = `
            <div id="hero-section"></div>
            <nav id="categories-nav"></nav> <!-- Add categoriesNav here -->
            <button class="add-category-btn" style="display: none;"></button> <!-- Ensure this exists if used -->
        `;
    });

    it('should add a category successfully and show a success message', async () => {
        // Arrange: Set up mock fetch response
        const mockCategoryData = { name: 'Category 1', description: 'A sample category', bottomDescription: 'Some additional info' };
        const mockResponseData = { id: 1, ...mockCategoryData };

        // Mock the fetch response
        fetch.mockResolvedValueOnce({
            ok: true,
            json: async () => mockResponseData,
        });

        // Act: Call the addCategory function
        const response = await addCategory(mockCategoryData);

        // Assert: Check if category was added and success message shown
        expect(response).toEqual(mockResponseData);
        expect(fetch).toHaveBeenCalledWith("https://localhost:7225/api/Category", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(mockCategoryData),
        });
        expect(showMessage).toHaveBeenCalledWith("Category added successfully!", true);
    });

    it('should handle errors and show an error message when category addition fails', async () => {
        // Arrange: Set up mock fetch to simulate failure
        const mockCategoryData = { name: 'Category 1', description: 'A sample category', bottomDescription: 'Some additional info' };
        fetch.mockResolvedValueOnce({ ok: false, status: 500 });

        // Act: Call the addCategory function
        await addCategory(mockCategoryData);

        // Assert: Check if error message was shown
        expect(showMessage).toHaveBeenCalledWith("Failed to add Category", false);
    });
});
