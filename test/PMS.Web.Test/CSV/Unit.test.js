/**
 * @jest-environment jsdom
 */

import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";

// Polyfill dialog.showModal for jsdom
if (!HTMLDialogElement.prototype.showModal) {
    HTMLDialogElement.prototype.showModal = function () {
        this.setAttribute("open", "true");
    };
}

if (!HTMLDialogElement.prototype.close) {
    HTMLDialogElement.prototype.close = function () {
        this.removeAttribute("open");
    };
}

// Import the showModal function after the polyfill
import { showModal } from "../../../src/PMS.Web/wwwroot/Javascript/Main/MainCsv.js";

describe("showModal", () => {
    let onConfirm;

    beforeEach(() => {
        onConfirm = vi.fn();
    });

    afterEach(() => {
        document.body.innerHTML = "";
    });

    it("should display modal with the correct message", () => {
        const message = "Are you sure you want to delete?";
        showModal(message, onConfirm);

        const modal = document.querySelector("dialog.delete-dialog");
        expect(modal).not.toBeNull();

        const modalMessage = modal.querySelector("p");
        expect(modalMessage.textContent).toBe(message);
    });

    it("should call onConfirm and remove modal when Confirm button is clicked", () => {
        const message = "Confirm delete?";
        showModal(message, onConfirm);

        const confirmButton = document.getElementById("confirm-btn");
        confirmButton.click();

        expect(onConfirm).toHaveBeenCalledOnce();
        expect(document.querySelector("dialog.delete-dialog")).toBeNull();
    });

    it("should close the modal and not call onConfirm when Cancel button is clicked", () => {
        const message = "Confirm delete?";
        showModal(message, onConfirm);

        const cancelButton = document.getElementById("cancel-btn");
        cancelButton.click();

        expect(onConfirm).not.toHaveBeenCalled();
        expect(document.querySelector("dialog.delete-dialog")).toBeNull();
    });
});
