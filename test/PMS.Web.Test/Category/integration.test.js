/**
 * @jest-environment jsdom
 */

import { describe, it, expect, vi, beforeEach} from "vitest";
import { deleteCategory } from "../../../src/PMS.Web/wwwroot/Javascript/Category/DeleteCategory.js";
import { renderAllCategories } from "../../../src/PMS.Web/wwwroot/Javascript/Main/MainCategory.js";
import { getAllCategories } from "../../../src/PMS.Web/wwwroot/Javascript/Category/GetCategories.js";
import { showMessage } from "../../../src/PMS.Web/wwwroot/Components/MessageBox.js";

// Mock functions
vi.mock("../../../src/PMS.Web/wwwroot/Components/MessageBox.js", () => ({
    showMessage: vi.fn(),
}));

vi.mock("../../../src/PMS.Web/wwwroot/Javascript/Main/MainCategory.js", () => ({
    renderAllCategories: vi.fn(),
}));

vi.mock("../../../src/PMS.Web/wwwroot/Javascript/Category/GetCategories.js", () => ({
    getAllCategories: vi.fn(),
}));

global.fetch = vi.fn();

describe("deleteCategory", () => {
    const categoryId = "123";

    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("should delete category successfully and update the list", async () => {
        // Arrange: Mock a successful fetch and updated categories
        fetch.mockResolvedValueOnce({ ok: true });
        const mockUpdatedCategories = [{ id: "1", name: "Category 1" }];
        getAllCategories.mockResolvedValueOnce(mockUpdatedCategories);

        // Act: Call deleteCategory
        await deleteCategory(categoryId);

        // Assert: Check if fetch was called with correct URL and method
        expect(fetch).toHaveBeenCalledWith(`https://localhost:7225/api/Category/${categoryId}`, { method: "DELETE" });

        // Check if showMessage was called with success message
        expect(showMessage).toHaveBeenCalledWith("Category deleted successfully!", true);

        // Check if getAllCategories and renderAllCategories were called
        expect(getAllCategories).toHaveBeenCalled();
        expect(renderAllCategories).toHaveBeenCalledWith(mockUpdatedCategories);
    });

    it("should show error message if deletion fails", async () => {
        // Arrange: Mock a failed fetch response
        fetch.mockResolvedValueOnce({ ok: false });

        // Act: Call deleteCategory
        await deleteCategory(categoryId);

        // Assert: Verify fetch was called with correct URL and method
        expect(fetch).toHaveBeenCalledWith(`https://localhost:7225/api/Category/${categoryId}`, { method: "DELETE" });

        // Check if showMessage was called with failure message
        expect(showMessage).toHaveBeenCalledWith("Failed to delete Category", false);

        // Ensure getAllCategories and renderAllCategories were not called
        expect(getAllCategories).not.toHaveBeenCalled();
        expect(renderAllCategories).not.toHaveBeenCalled();
    });

    it("should handle fetch errors and show error message", async () => {
        // Arrange: Mock fetch to throw an error
        fetch.mockRejectedValueOnce(new Error("Network error"));

        // Act: Call deleteCategory
        await deleteCategory(categoryId);

        // Assert: Verify that fetch was called
        expect(fetch).toHaveBeenCalledWith(`https://localhost:7225/api/Category/${categoryId}`, { method: "DELETE" });

        // Check if showMessage was called with failure message
        expect(showMessage).toHaveBeenCalledWith("Failed to delete Category", false);

        // Ensure getAllCategories and renderAllCategories were not called
        expect(getAllCategories).not.toHaveBeenCalled();
        expect(renderAllCategories).not.toHaveBeenCalled();
    });
});
