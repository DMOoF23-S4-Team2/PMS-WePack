import { showMessage } from "../../Components/MessageBox.js";


let categories = [];

export async function getAllCategories() {
    try {
        const res = await fetch("https://localhost:7225/api/Category/categories");

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