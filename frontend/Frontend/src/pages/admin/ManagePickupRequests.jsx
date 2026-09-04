import { useEffect, useState } from "react";
import {
    getPickupRequests,
    approvePickupRequest,
    declinePickupRequest
} from "../../services/pickupRequestService";

function ManagePickupRequests() {

    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [actionLoading, setActionLoading] = useState(null);

    const loadRequests = async () => {
        try {
            setLoading(true);
            setError("");

            const data = await getPickupRequests();

            setRequests(data);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadRequests();
    }, []);

    const handleApprove = async (id) => {

        const confirmed = window.confirm(
            "Are you sure you want to approve this pickup request?"
        );

        if (!confirmed) {
            return;
        }

        try {
            setActionLoading(id);
            setError("");

            await approvePickupRequest(id);

            await loadRequests();

        } catch (error) {
            setError(error.message);
        } finally {
            setActionLoading(null);
        }
    };

    const handleDecline = async (id) => {

        const confirmed = window.confirm(
            "Are you sure you want to decline this pickup request?"
        );

        if (!confirmed) {
            return;
        }

        try {
            setActionLoading(id);
            setError("");

            await declinePickupRequest(id);

            await loadRequests();

        } catch (error) {
            setError(error.message);
        } finally {
            setActionLoading(null);
        }
    };

    if (loading) {
        return (
            <div className="page-container">
                <h2>Pickup Requests</h2>
                <p>Loading pickup requests...</p>
            </div>
        );
    }

    return (
        <div className="page-container">

            <h1>Pickup Requests</h1>

            {error && (
                <div className="error-message">
                    {error}
                </div>
            )}

            {requests.length === 0 ? (
                <p>No pickup requests found.</p>
            ) : (
                <div className="requests-container">

                    {requests.map((request) => (

                        <div
                            className="request-card"
                            key={request.id}
                        >

                            <div className="request-header">

                                <h3>
                                    Request #{request.id}
                                </h3>

                                <span
                                    className={`status ${request.status.toLowerCase()}`}
                                >
                                    {request.status}
                                </span>

                            </div>

                            <div className="request-details">

                                <p>
                                    <strong>Customer:</strong>{" "}
                                    {request.customerName}
                                </p>

                                <p>
                                    <strong>Email:</strong>{" "}
                                    {request.email}
                                </p>

                                <p>
                                    <strong>Item:</strong>{" "}
                                    {request.itemType}
                                </p>

                                <p>
                                    <strong>Quantity:</strong>{" "}
                                    {request.quantity}
                                </p>

                                <p>
                                    <strong>Location:</strong>{" "}
                                    {request.location}
                                </p>

                                <p>
                                    <strong>Preferred Date:</strong>{" "}
                                    {new Date(
                                        request.preferredDate
                                    ).toLocaleDateString()}
                                </p>

                            </div>

                            {request.status === "Pending" && (

                                <div className="request-actions">

                                    <button
                                        className="approve-btn"
                                        onClick={() =>
                                            handleApprove(request.id)
                                        }
                                        disabled={
                                            actionLoading === request.id
                                        }
                                    >
                                        {actionLoading === request.id
                                            ? "Processing..."
                                            : "Approve"}
                                    </button>

                                    <button
                                        className="decline-btn"
                                        onClick={() =>
                                            handleDecline(request.id)
                                        }
                                        disabled={
                                            actionLoading === request.id
                                        }
                                    >
                                        {actionLoading === request.id
                                            ? "Processing..."
                                            : "Decline"}
                                    </button>

                                </div>

                            )}

                        </div>

                    ))}

                </div>
            )}

        </div>
    );
}

export default ManagePickupRequests;