import { showMessage } from "../../Components/MessageBox.js"
import { getAllProducts } from "./GetProducts.js";
import { renderAllProducts } from "../Main/MainProduct.js";

export async function updateProduct(productId, updatedData) {
    try {
        const response = await fetch(`https://localhost:7225/api/Product/${productId}`, {
            method: 'PUT', 
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedData)  // Send the updated product data in the request body
        });

        if (!response.ok) {
            console.error(`Failed to update product with ID ${productId}.`);
            showMessage(`Failed to update product`, false);  // Show error message
            return;
        }

        console.log(`Product with ID ${productId} updated successfully.`);
        showMessage("Product updated successfully!", true);  // Show success message

    } catch (error) {
        console.error('Error updating product:', error);
        showMessage(`Failed to update product`, false);
    }
}

export async function showUpdateModal(productId, updateProductCallback) {

    // Fetch the product data based on the productId
    const response = await fetch(`https://localhost:7225/api/Product/${productId}`);
    const product = await response.json();  // Assuming the product data is in the response body

    // Create the <dialog> element
    const updateDialog = document.createElement('dialog');
    updateDialog.innerHTML = `
        
         <form id="update-product-form">
            <div class="form-container">                
                <label for="sku">SKU</label>
                <input required id="sku" name="sku" value="${product.sku}">

                <label for="ean">EAN</label>
                <input id="ean" name="ean" value="${product.ean}">

                <label for="name">Name</label>
                <input required id="name" name="name" value="${product.name}">

                <label for="description">Description</label>
                <textarea id="description" name="description">${product.description}</textarea>  
                
                <label for="category">Category</label>
                <input id="category" name="category" value="${product.category}">

                <div class="units-container">
                    <div>
                        <label for="price">Price</label>
                        <input required id="price" type="number" name="price" value="${product.price}">
                    </div>
                    <div>
                        <label for="specialPrice">Special Price</label>
                        <input id="specialPrice" type="number" name="specialPrice" value="${product.specialPrice || ''}">
                    </div>    
                </div> 

                <label for="supplier">Supplier</label>
                <input id="supplier" name="supplier" value="${product.supplier}">

                <label for="supplierSku">Supplier SKU</label>
                <input id="supplierSku" name="supplierSku" value="${product.supplierSku}">
                
            </div>
            <div class="form-container">                
                <label for="templateNo">Template No</label>
                <input id="templateNo" type="number" name="templateNo" value="${product.templateNo || ''}">

                <label for="productType">Product type</label>
                <input id="productType" name="productType" value="${product.productType}">

                <label for="productGroup">Product group</label>
                <input id="productGroup" name="productGroup" value="${product.productGroup}">

                <label for="currency">Currency</label>
                <input id="currency" name="currency" value="${product.currency}">

                <label for="material">Material</label>
                <input id="material" name="material" value="${product.material}">

                <label for="color">Color</label>
                <input id="color" name="color" value="${product.color}">

                <label for="list">List</label>
                <input id="list" type="number" name="list" value="${product.list || ''}">

                <div class="units-container">
                    <div>
                        <label for="weight">Weight</label>
                        <input id="weight" type="number" name="weight" value="${product.weight || ''}">
                    </div>
                    <div>
                        <label for="cost">Cost</label>
                        <input id="cost" type="number" name="cost" value="${product.cost || ''}">
                    </div>    
                </div>    

                <div class="dialog-actions">
                    <button type="submit" class="yes-update-btn">Update</button>
                    <button class="no-update-btn">Cancel</button>
                </div>
            </div>     
        </form>
    `;
    
    // Append the dialog to the document body
    document.body.appendChild(updateDialog);

    // Show the dialog
    updateDialog.showModal();

    // // Get references to Yes and No buttons
    // const yesBtn = updateDialog.querySelector('.yes-update-btn');
    const noBtn = updateDialog.querySelector('.no-update-btn');
    const updateForm = document.getElementById('update-product-form');

    updateForm.addEventListener('submit', async (e) => {
        e.preventDefault();  // Prevent form from refreshing the page

        let updatedData = new FormData(updateForm)

        // Get the updated data from the form inputs
        const Data = {
            productDto: {
                sku: updatedData.get('sku'),
                ean: updatedData.get('ean'),
                name: updatedData.get('name'),
                description: updatedData.get('description'),
                price: parseFloat(updatedData.get('price')), 
                specialPrice: parseFloat(updatedData.get('specialPrice')) || 0,
                productType: updatedData.get('productType'),
                productGroup: updatedData.get('productGroup'),
                currency: updatedData.get('currency'),
                material: updatedData.get('material'),
                color: updatedData.get('color'),
                supplier: updatedData.get('supplier'),
                supplierSku: updatedData.get('supplierSku'),
                templateNo: parseInt(updatedData.get('templateNo')) || 0,
                list: parseInt(updatedData.get('list')) || 0,
                weight: parseFloat(updatedData.get('weight')) || 0,
                cost: parseFloat(updatedData.get('cost')) || 0  
            }
        };
        
        // Call the updateProduct function with the product ID and updated data
        await updateProductCallback(productId, Data.productDto);

        // Fetch updated product list and re-render it
        const updatedProducts = await getAllProducts();
        renderAllProducts(updatedProducts);


        updateDialog.close(); // Close the dialog
        updateDialog.remove(); // Remove the dialog from the DOM
    });

    // Handle the No button click
    noBtn.addEventListener('click', () => {
        updateDialog.close(); // Close the dialog
        updateDialog.remove(); // Remove the dialog from the DOM
    });
}
