const API_URL = "http://localhost:5000/api/pickup-requests";

// Get all pickup requests
export const getPickupRequests = async () => {
    const response = await fetch(API_URL);

    if (!response.ok) {
        throw new Error("Failed to load pickup requests.");
    }

    return await response.json();
};

// Approve pickup request
export const approvePickupRequest = async (id) => {
    const response = await fetch(`${API_URL}/${id}/approve`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        }
    });

    const data = await response.json();

    if (!response.ok) {
        throw new Error(data.message || "Failed to approve request.");
    }

    return data;
};

// Decline pickup request
export const declinePickupRequest = async (id) => {
    const response = await fetch(`${API_URL}/${id}/decline`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        }
    });

    const data = await response.json();

    if (!response.ok) {
        throw new Error(data.message || "Failed to decline request.");
    }

    return data;
};