/**
 * @jest-environment jsdom
 */

import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { getAllProducts } from '../../../src/PMS.Web/wwwroot/Javascript/Product/GetProducts.js';

// Mock the global fetch function
global.fetch = vi.fn();

describe('getAllProducts', () => {
    beforeEach(() => {
        // Set up a hero-section in the DOM before each test
        const heroSection = document.createElement('div');
        heroSection.id = 'hero-section';
        document.body.appendChild(heroSection);
    });

    afterEach(() => {
        // Clean up the DOM after each test
        document.body.innerHTML = '';
        fetch.mockClear();
    });

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
