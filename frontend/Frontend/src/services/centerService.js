import axios from 'axios';

// Base URL for the API - adjust if your backend runs on a different port
const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:5184/api';

const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    timeout: 10000, // 10 seconds timeout
});

// Response interceptor for better error handling
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        // Extract and format error message
        if (error.response) {
            // Server responded with an error status
            const message = error.response.data?.message || 
                           error.response.data?.title || 
                           error.response.data?.errors || 
                           'An error occurred';
            
            // Handle validation errors
            if (error.response.status === 400 && error.response.data?.errors) {
                const validationErrors = error.response.data.errors;
                const firstError = Object.values(validationErrors)[0];
                error.message = Array.isArray(firstError) ? firstError[0] : 'Validation error';
            } else {
                error.message = typeof message === 'string' ? message : 'An error occurred';
            }
        } else if (error.request) {
            // Request was made but no response received
            error.message = 'Unable to connect to the server. Please check your connection.';
        } else {
            // Something else happened
            error.message = 'An unexpected error occurred.';
        }
        return Promise.reject(error);
    }
);

/**
 * Service for managing collection centers
 */
const centerService = {
    /**
     * Get all collection centers
     * @returns {Promise<Array>} List of centers
     */
    async getCenters() {
        try {
            const response = await apiClient.get('/Centers');
            return response.data;
        } catch (error) {
            console.error('Error fetching centers:', error);
            throw error;
        }
    },

    /**
     * Get a specific center by ID
     * @param {number} id - Center ID
     * @returns {Promise<Object>} Center data
     */
    async getCenterById(id) {
        try {
            const response = await apiClient.get(`/Centers/${id}`);
            return response.data;
        } catch (error) {
            console.error(`Error fetching center ${id}:`, error);
            throw error;
        }
    },

    /**
     * Create a new collection center
     * @param {Object} centerData - Center data
     * @returns {Promise<Object>} Created center
     */
    async createCenter(centerData) {
        try {
            const response = await apiClient.post('/Centers', centerData);
            return response.data;
        } catch (error) {
            console.error('Error creating center:', error);
            throw error;
        }
    },

    /**
     * Update an existing collection center
     * @param {number} id - Center ID
     * @param {Object} centerData - Updated center data
     * @returns {Promise<Object>} Updated center
     */
    async updateCenter(id, centerData) {
        try {
            const response = await apiClient.put(`/Centers/${id}`, centerData);
            return response.data;
        } catch (error) {
            console.error(`Error updating center ${id}:`, error);
            throw error;
        }
    },

    /**
     * Delete a collection center
     * @param {number} id - Center ID
     * @returns {Promise<void>}
     */
    async deleteCenter(id) {
        try {
            await apiClient.delete(`/Centers/${id}`);
        } catch (error) {
            console.error(`Error deleting center ${id}:`, error);
            throw error;
        }
    }
};

export default centerService;