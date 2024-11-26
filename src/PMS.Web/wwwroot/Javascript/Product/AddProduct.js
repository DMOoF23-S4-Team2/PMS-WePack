import { showMessage } from "../../Components/MessageBox.js"
import { renderAllProducts } from "../Main/MainProduct.js";
import { getAllProducts } from "./GetProducts.js";


export async function addProduct(productData) {
    try {
        const response = await fetch("https://localhost:7225/api/Product", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"  
            },
            body: JSON.stringify(productData)
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const data = await response.json();
        console.log("Product added successfully:", data);

         showMessage("Product added successfully!", true);

        return data;
    } catch (error) {
        console.error("Failed to add product:", error.message);

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
                price: parseFloat(productData.get('price')), 
                specialPrice: parseFloat(productData.get('specialPrice')) || 0, 
                productType: productData.get('productType'),
                productGroup: productData.get('productGroup'),
                currency: productData.get('currency'),
                material: productData.get('material'),
                color: productData.get('color'),
                supplier: productData.get('supplier'),
                supplierSku: productData.get('supplierSku'),
                templateNo: parseInt(productData.get('templateNo')) || 0, 
                list: parseInt(productData.get('list')) || 0, 
                weight: parseFloat(productData.get('weight')) || 0, 
                cost: parseFloat(productData.get('cost')) || 0 
            }
        };

        console.log("Data being sent:", Data);

        try {
            await addProduct(Data.productDto);  

            dialog.close();  
            dialog.remove();  

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
                        <input required id="price" step="any" type="number" name="price">
                    </div>
                    <div>
                        <label for="specialPrice">Special Price</label>
                        <input id="specialPrice" step="any" type="number" name="specialPrice">
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
                        <input id="weight" step="any" type="number" name="weight">
                    </div>
                    <div>
                        <label for="cost">Cost</label>
                        <input id="cost" step="any" type="number" name="cost">
                    </div>    
                </div>    

                <div class="dialog-actions">
                    <button type="submit" class="confirm-add-btn">Add</button>
                    <button type="button" class="close-modal-btn">Cancel</button>
                </div>
            </div>     
        </form>
    `;

    document.body.appendChild(addProductDialog); 
    addProductDialog.showModal();  // Show the modal

    // Add form handler for submitting the form
    addProductFormHandler(addProductDialog);

    // Close modal functionality
    const closeModalBtn = addProductDialog.querySelector('.close-modal-btn');
    closeModalBtn.addEventListener('click', () => {
        addProductDialog.close();
        addProductDialog.remove(); 
    });
}


