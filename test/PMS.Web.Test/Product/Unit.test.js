/**
 * @jest-environment jsdom
 */

import { describe, it, expect, beforeEach } from 'vitest';
import { showMessage } from '../../../src/PMS.Web/wwwroot/Components/MessageBox.js';

describe('showMessage Function', () => {
  // Set up the necessary HTML structure before each test
  beforeEach(() => {
      document.body.innerHTML = `
          <div id="hero-section"></div>
      `;
  });

  it('should create a success message with correct text and background', () => {
      showMessage('Success!', true);

      const messageContainer = document.querySelector('.message-container');
      expect(messageContainer).not.toBeNull(); // Check that the message container was created
      expect(messageContainer.querySelector('p').textContent).toBe('Success!'); // Check message text
      expect(messageContainer.style.background).toBe('green'); // Check background color
  });
});
