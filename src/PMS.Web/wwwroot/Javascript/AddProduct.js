export function addProduct(productData) {

    fetch("https://localhost:7225/api/Product", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"  // Specify that we're sending JSON
        },
        body: JSON.stringify(productData)  // Convert the productData object to a JSON string
    })
    .then(res => res.json())
    .then(data => data)
}