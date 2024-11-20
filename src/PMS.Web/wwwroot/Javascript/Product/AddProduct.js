import { showMessage } from "../../Components/MessageBox.js"
import { renderAllProducts } from "../Main/MainProduct.js";
import { getAllProducts } from "./GetProducts.js";
import { getApiUrl } from "../config.js";

export async function addProduct(productData) {
    try {
        const API_URL = await getApiUrl();
        const response = await fetch(`${API_URL}/api/Product`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"  // Specify that we're sending JSON
            },
            body: JSON.stringify(productData)
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const data = await response.json();
        console.log("Product added successfully:", data);

         // Show success message
         showMessage("Product added successfully!", true);

        return data;
    } catch (error) {
        console.error("Failed to add product:", error.message);

        // Show error message
        showMessage(`Failed to add product`, false);
    }
}

export function addProductFormHandler(dialog) {
    const form = document.getElementById("add-product-form");

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        let productData = new FormData(form);

        const Data = {
            productDto: {
                sku: productData.get('sku'),
                ean: productData.get('ean'),
                name: productData.get('name'),
                description: productData.get('description'),
                price: parseFloat(productData.get('price')),  // Parse as float
                specialPrice: parseFloat(productData.get('specialPrice')) || 0,  // Default to 0 if empty
                productType: productData.get('productType'),
                productGroup: productData.get('productGroup'),
                currency: productData.get('currency'),
                material: productData.get('material'),
                color: productData.get('color'),
                supplier: productData.get('supplier'),
                supplierSku: productData.get('supplierSku'),
                templateNo: parseInt(productData.get('templateNo')) || 0,  // Default to 0 if empty
                list: parseInt(productData.get('list')) || 0,  // Default to 0 if empty
                weight: parseFloat(productData.get('weight')) || 0,  // Default to 0 if empty
                cost: parseFloat(productData.get('cost')) || 0  // Default to 0 if empty
            }
        };

        console.log("Data being sent:", Data);

        try {
            await addProduct(Data.productDto);  // Wait for the product to be added

            dialog.close();  // Close the dialog
            dialog.remove();  // Remove dialog from DOM

            const updatedProducts = await getAllProducts();  // Fetch updated categories
            renderAllProducts(updatedProducts);  // Rerender the table with new categories


            form.reset();  // Reset the form after successful submission
        } catch (error) {
            console.error("Error adding product:", error);
        }
    });
}

export function renderAddProductModal() {

    // Check if a dialog already exists and remove it to avoid multiple dialogs
    const existingDialog = document.querySelector('dialog[open]');
    if (existingDialog) {
        existingDialog.close();
        existingDialog.remove();
    }

    const addProductDialog = document.createElement('dialog');
    
    // Create the <dialog> element    
    addProductDialog.innerHTML = `
        
         <form id="add-product-form" class="product-dialog">
            <div class="form-container">                
                <label for="sku">SKU</label>
                <input required id="sku" name="sku">

                <label for="ean">EAN</label>
                <input id="ean" name="ean">

                <label for="name">Name</label>
                <input required id="name" name="name">

                <label for="description">Description</label>
                <textarea id="description" name="description"></textarea>  
                
                <label for="category">Category</label>
                <input id="category" name="category">

                <div class="units-container">
                    <div>
                        <label for="price">Price</label>
                        <input required id="price" type="number" name="price">
                    </div>
                    <div>
                        <label for="specialPrice">Special Price</label>
                        <input id="specialPrice" type="number" name="specialPrice">
                    </div>    
                </div> 

                <label for="supplier">Supplier</label>
                <input id="supplier" name="supplier">

                <label for="supplierSku">Supplier SKU</label>
                <input id="supplierSku" name="supplierSku">
                
            </div>
            <div class="form-container">                
                <label for="templateNo">Template No</label>
                <input id="templateNo" type="number" name="templateNo">

                <label for="productType">Product type</label>
                <input id="productType" name="productType">

                <label for="productGroup">Product group</label>
                <input id="productGroup" name="productGroup">

                <label for="currency">Currency</label>
                <input id="currency" name="currency">

                <label for="material">Material</label>
                <input id="material" name="material">

                <label for="color">Color</label>
                <input id="color" name="color">

                <label for="list">List</label>
                <input id="list" type="number" name="list">

                <div class="units-container">
                    <div>
                        <label for="weight">Weight</label>
                        <input id="weight" type="number" name="weight">
                    </div>
                    <div>
                        <label for="cost">Cost</label>
                        <input id="cost" type="number" name="cost">
                    </div>    
                </div>    

                <div class="dialog-actions">
                    <button type="submit" class="confirm-add-btn">Add</button>
                    <button type="button" class="close-modal-btn">Cancel</button>
                </div>
            </div>     
        </form>
    `;

    document.body.appendChild(addProductDialog);  // Append the modal to the DOM
    addProductDialog.showModal();  // Show the modal

    // Add form handler for submitting the form
    addProductFormHandler(addProductDialog);

    // Close modal functionality
    const closeModalBtn = addProductDialog.querySelector('.close-modal-btn');
    closeModalBtn.addEventListener('click', () => {
        addProductDialog.close();
        addProductDialog.remove();  // Remove the dialog after closing
    });
}


