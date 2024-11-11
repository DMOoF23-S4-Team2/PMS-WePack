/**
 * @jest-environment jsdom
 */

import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { uploadCsv } from '../../../src/PMS.Web/wwwroot/Javascript/Csv/UploadCsv.js';
import { showMessage } from '../../../src/PMS.Web/wwwroot/Components/MessageBox.js';

// Mock showMessage from the imported file
vi.mock('../../../src/PMS.Web/wwwroot/Components/MessageBox.js', () => ({
    showMessage: vi.fn(),
}));

// Mock global fetch
global.fetch = vi.fn();

describe('uploadCsv', () => {
    beforeEach(() => {
        fetch.mockClear();
        showMessage.mockClear();
    });

    it('should upload CSV successfully and show a success message', async () => {
        // Arrange
        const filePath = "C:\\Users\\User\\Documents\\create-test.csv";
        const mockResponseText = "File uploaded successfully";

        fetch.mockResolvedValueOnce({
            ok: true,
            text: async () => mockResponseText,
        });

        // Act
        const result = await uploadCsv(filePath);

        // Assert
        expect(result).toBe(mockResponseText);
        expect(fetch).toHaveBeenCalledWith(
            `https://localhost:7225/api/Csv/upload-csv?filepath=${filePath}`,
            {
                method: "POST",
                headers: { "Content-Type": "text/plain" },
                body: filePath
            }
        );
        expect(showMessage).toHaveBeenCalledWith("CSV added successfully!", true);
    });

    it('should handle errors and show an error message on failed upload', async () => {
        // Arrange
        const filePath = "C:\\Users\\User\\Documents\\update-test.csv";

        fetch.mockResolvedValueOnce({
            ok: false,
            status: 404,
        });

        // Act
        const result = await uploadCsv(filePath);

        // Assert
        expect(result).toBeUndefined();
        expect(fetch).toHaveBeenCalledWith(
            `https://localhost:7225/api/Csv/upload-csv?filepath=${filePath}`,
            {
                method: "POST",
                headers: { "Content-Type": "text/plain" },
                body: filePath
            }
        );
        expect(showMessage).toHaveBeenCalledWith("Failed to update Csv!", false);
    });

    it('should handle network errors and show an error message', async () => {
        // Arrange
        const filePath = "C:\\Users\\User\\Documents\\delete-test.csv";

        fetch.mockRejectedValueOnce(new Error("Network error"));

        // Act
        const result = await uploadCsv(filePath);

        // Assert
        expect(result).toBeUndefined();
        expect(showMessage).toHaveBeenCalledWith("Failed to delete Csv!", false);
    });
});
