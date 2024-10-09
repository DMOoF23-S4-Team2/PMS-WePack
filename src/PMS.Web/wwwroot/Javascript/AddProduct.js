// export function addProduct(productData) {

//     fetch("https://localhost:7225/api/Product", {
//         method: "POST",
//         headers: {
//             "Content-Type": "application/json"  // Specify that we're sending JSON
//         },
//         body: JSON.stringify(productData)  // Convert the productData object to a JSON string
//     })
//     .then(res => res.json())
//     .then(data => data)
// }
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
        return data;
    } catch (error) {
        console.error("Failed to add product:", error.message);
    }
}
