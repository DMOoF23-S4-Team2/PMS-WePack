let configPromise = null;

// Function to fetch the configuration
async function fetchConfig() {
	try {
		const response = await fetch("/config");
		if (!response.ok) {
			throw new Error(
				`Failed to fetch configuration: ${response.statusText}`
			);
		}

		const config = await response.json();

		// Ensure the key is correctly mapped
		if (!config.apiUrl) {
			throw new Error("Missing 'apiUrl' in configuration response.");
		}

		return config;
	} catch (error) {
		console.error("Error loading configuration:", error);
		// Provide a fallback value for development
		return {
			apiUrl: "http://localhost:5184",
		};
	}
}

// Singleton to fetch the configuration only once
function getConfig() {
	if (!configPromise) {
		configPromise = fetchConfig();
	}
	return configPromise;
}

// Async function to get the API URL
export async function getApiUrl() {
	const config = await getConfig();
	return config.apiUrl;
}
