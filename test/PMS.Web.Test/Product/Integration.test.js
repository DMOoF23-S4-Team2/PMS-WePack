//CODE TO TEST DOM INTERACTIONS
/**
 * @jest-environment jsdom
 */

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { getAllProducts } from '../../../src/PMS.Web/wwwroot/Javascript/GetProducts.js';
import { addProduct } from '../../../src/PMS.Web/wwwroot/Javascript/AddProduct.js';
import { showMessage } from '../../../src/PMS.Web/wwwroot/Components/MessageBox.js';

// Mock the global fetch function
global.fetch = vi.fn();

vi.mock('../../../src/PMS.Web/wwwroot/Components/MessageBox.js', () => ({
    showMessage: vi.fn(),  // Mock showMessage function
}));

// TESTING GET PRODUCTS API FETCH

describe('getAllProducts', () => {
    it('should fetch products successfully and return them', async () => {
        // Arrange: Set up the mock fetch response
        const mockProducts = [{ id: 1, sku: 'SKU001', name: 'Product 1', price: 100, currency: 'USD' }];
        fetch.mockResolvedValueOnce({
            ok: true,
            json: async () => mockProducts,
        });

        // Act: Call the function
        const products = await getAllProducts();

        // Assert: Check if the products were fetched and returned correctly
        expect(products).toEqual(mockProducts);
        expect(fetch).toHaveBeenCalledWith("https://localhost:7225/api/Product/products");
    });

    it('should handle errors when fetch fails', async () => {
        // Arrange: Set up the mock fetch to simulate an error
        fetch.mockResolvedValueOnce({ ok: false, status: 404 });

        // Act: Call the function
        const products = await getAllProducts();

        // Assert: Check if products is undefined when there is an error
        expect(products).toBeUndefined();
        expect(fetch).toHaveBeenCalledWith("https://localhost:7225/api/Product/products");
    });
});

// TESTING ADD PRODUCT API FETCH

describe('addProduct', () => {

        // Add DOM setup
    beforeEach(() => {
        // Set up the DOM structure that your function relies on
        document.body.innerHTML = `
            <div id="hero-section"></div>
        `;
    });

    it('should add a product successfully and show a success message', async () => {
        // Arrange: Set up mock fetch response
        const mockProductData = { id: 1, name: 'Product 1', price: 100, sku: 'SKU001' };
        const mockResponseData = { id: 1, name: 'Product 1', price: 100, sku: 'SKU001' };
        
        fetch.mockResolvedValueOnce({
            ok: true,
            json: async () => mockResponseData,
        });

        // Act: Call the addProduct function
        const response = await addProduct(mockProductData);

        // Assert: Check if product was added and success message shown
        expect(response).toEqual(mockResponseData);
        expect(fetch).toHaveBeenCalledWith("https://localhost:7225/api/Product", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(mockProductData),
        });
        expect(showMessage).toHaveBeenCalledWith("Product added successfully!", true);
    });

    it('should handle errors and show an error message when product addition fails', async () => {
        // Arrange: Set up mock fetch to simulate failure
        const mockProductData = { name: 'Product 1', price: 100, sku: 'SKU001' };
        fetch.mockResolvedValueOnce({ ok: false, status: 500 });

        // Act: Call the addProduct function
        await addProduct(mockProductData);

        // Assert: Check if error message was shown
        expect(showMessage).toHaveBeenCalledWith("Failed to add product", false);
    });
});
