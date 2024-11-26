import { showMessage } from "../../Components/MessageBox.js";
import { getApiUrl } from "../config.js";

let categories = [];

export async function getAllCategories() {
    try {
		const API_URL = await getApiUrl();
		const res = await fetch(`${API_URL}/api/Category/categories`);

      if (!res.ok) {
          throw new Error(`HTTP error! status: ${res.status}`);
      }

		const data = await res.json();
		categories = data;

        return categories;
    } catch (error) {

        console.error("Error fetching categories:", error);
        showMessage("Error fetching Categories", false)
    }
}