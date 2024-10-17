import { showMessage } from "../../Components/MessageBox.js"

export async function addProduct(productData) {
    try {
        const response = await fetch("https://localhost:7225/api/Product", {
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

export function addProductFormHandler() {
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
            form.reset();  // Reset the form after successful submission
        } catch (error) {
            console.error("Error adding product:", error);
        }
    });
}


