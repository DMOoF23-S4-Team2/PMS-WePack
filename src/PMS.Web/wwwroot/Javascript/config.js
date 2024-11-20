// let configPromise = null;

// // Function to fetch the configuration
// async function fetchConfig() {
// 	try {
// 		const response = await fetch("/config");
// 		if (!response.ok) {
// 			throw new Error(
// 				`Failed to fetch configuration: ${response.statusText}`
// 			);
// 		}
// 		const config = await response.json();
// 		return config;
// 	} catch (error) {
// 		console.error("Error loading configuration:", error);
// 		// Provide a fallback value for development
// 		return {
// 			ApiUrl: "http://localhost:7225",
// 		};
// 	}
// }

// // Singleton to get the configuration (fetches only once)
// function getConfig() {
// 	if (!configPromise) {
// 		configPromise = fetchConfig();
// 	}
// 	return configPromise;
// }

// export async function getApiUrl() {
// 	const config = await getConfig();
// 	return config.ApiUrl;
// }

export const API_URL = "http://localhost:7225"; // Hardcoded API URL for development