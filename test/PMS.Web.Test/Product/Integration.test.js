//CODE TO TEST DOM INTERACTIONS
/**
 * @jest-environment jsdom
 */

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { getAllProducts } from '../../../src/PMS.Web/wwwroot/Javascript/Product/GetProducts.js';

// Mock the global fetch function
global.fetch = vi.fn();

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


