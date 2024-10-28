/**
 * @jest-environment jsdom
 */

import { describe, it, expect, beforeEach, afterEach, vi, beforeAll, afterAll } from "vitest";

import { renderAddCategoryModal } from "../../../src/PMS.Web/wwwroot/Javascript/Category/AddCategory.js";

// Mocking the dependencies
vi.mock("../../../src/PMS.Web/wwwroot/Javascript/Main/MainCategory", () => ({
    renderAllCategories: vi.fn(),
}));

vi.mock("../../../src/PMS.Web/wwwroot/Javascript/Product/GetCategories", () => ({
    getAllCategories: vi.fn(),
}));

// Mock showModal on all dialog elements for the test environment
beforeAll(() => {
    HTMLDialogElement.prototype.showModal = vi.fn();
});

afterAll(() => {
    // Restore the original showModal method after tests
    HTMLDialogElement.prototype.showModal.mockRestore();
});


describe("renderAddCategoryModal", () => {
    beforeEach(() => {
        document.body.innerHTML = ""; // Clear DOM before each test
    });

    afterEach(() => {
        vi.restoreAllMocks(); // Restore all mocks after each test
    });

    it("should render the modal with form elements", () => {
        // Act: Render the modal
        renderAddCategoryModal();

        // Assert: Check if the modal and form elements are in the DOM
        const dialog = document.querySelector("dialog");
        expect(dialog).not.toBeNull();  // Expect dialog to be rendered

        const nameInput = dialog.querySelector("input[name='name']");
        const descriptionInput = dialog.querySelector("textarea[name='description']");
        const bottomDescriptionInput = dialog.querySelector("textarea[name='bottomDescription']");
        const confirmButton = dialog.querySelector("button.confirm-add-btn");
        const cancelButton = dialog.querySelector("button.close-modal-btn");

        expect(nameInput).not.toBeNull();
        expect(descriptionInput).not.toBeNull();
        expect(bottomDescriptionInput).not.toBeNull();
        expect(confirmButton).not.toBeNull();
        expect(cancelButton).not.toBeNull();
    });

    it("should call addCategoryFormHandler when form is submitted", async () => {
        // Arrange: Render the modal and mock the addCategory function
        renderAddCategoryModal();

        const form = document.getElementById("add-category-form");
        const nameInput = form.querySelector("input[name='name']");
        const descriptionInput = form.querySelector("textarea[name='description']");
        
        // Simulate user input by directly setting input values
        nameInput.value = "Test Category";
        descriptionInput.value = "Test Description";

        // Mock the fetch function
        global.fetch = vi.fn().mockResolvedValueOnce({
            ok: true,
            json: async () => ({}),
        });

        // Act: Submit the form by dispatching a submit event
        const submitEvent = new Event("submit", { bubbles: true, cancelable: true });
        form.dispatchEvent(submitEvent);

        // Assert: Check if fetch was called with the correct data
        expect(global.fetch).toHaveBeenCalledWith("https://localhost:7225/api/Category", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                name: "Test Category",
                description: "Test Description",
                bottomDescription: "",
            }),
        });
    });

});
