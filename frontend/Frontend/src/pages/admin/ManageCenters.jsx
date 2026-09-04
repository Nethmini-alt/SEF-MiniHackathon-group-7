/* eslint-disable no-unused-vars */
/* eslint-disable react-hooks/immutability */
import React, { useState, useEffect } from 'react';
import centerService from '../../services/centerService';
import './ManageCenters.css';

const ManageCenters = () => {
    // State for centers list
    const [centers, setCenters] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [successMessage, setSuccessMessage] = useState(null);

    // State for form
    const [showForm, setShowForm] = useState(false);
    const [editingCenter, setEditingCenter] = useState(null);
    const [formData, setFormData] = useState({
        name: '',
        district: '',
        address: '',
        phone: '',
        acceptedItemType: '',
        openingHours: ''
    });
    const [formErrors, setFormErrors] = useState({});

    // State for delete confirmation
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [centerToDelete, setCenterToDelete] = useState(null);

    // State for search
    const [searchTerm, setSearchTerm] = useState('');

    // Fetch centers on component mount
    useEffect(() => {
        fetchCenters();
    }, []);

    // Clear success message after 3 seconds
    useEffect(() => {
        if (successMessage) {
            const timer = setTimeout(() => setSuccessMessage(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [successMessage]);

    // Clear error message after 5 seconds
    useEffect(() => {
        if (error) {
            const timer = setTimeout(() => setError(null), 5000);
            return () => clearTimeout(timer);
        }
    }, [error]);

    // Fetch all centers
    const fetchCenters = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await centerService.getCenters();
            setCenters(data);
        } catch (err) {
            setError('❌ Unable to load collection centers. Please try again.');
            console.error('Fetch centers error:', err);
        } finally {
            setLoading(false);
        }
    };

    // Handle form input changes
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
        // Clear error for this field when user starts typing
        if (formErrors[name]) {
            setFormErrors(prev => ({ ...prev, [name]: '' }));
        }
    };

    // Validate form
    const validateForm = () => {
        const errors = {};
        if (!formData.name.trim()) errors.name = 'Center name is required';
        if (!formData.district.trim()) errors.district = 'District is required';
        if (!formData.address.trim()) errors.address = 'Address is required';
        if (!formData.phone.trim()) {
            errors.phone = 'Phone number is required';
        } else if (!/^[\d+\s-()]{10,15}$/.test(formData.phone.trim())) {
            errors.phone = 'Please enter a valid phone number (10-15 digits)';
        }
        if (!formData.acceptedItemType.trim()) errors.acceptedItemType = 'Accepted item types are required';
        if (!formData.openingHours.trim()) errors.openingHours = 'Opening hours are required';
        return errors;
    };

    // Handle form submission (create or update)
    const handleSubmit = async (e) => {
        e.preventDefault();
        const errors = validateForm();
        if (Object.keys(errors).length > 0) {
            setFormErrors(errors);
            return;
        }

        try {
            let response;
            if (editingCenter) {
                // Update existing center
                response = await centerService.updateCenter(editingCenter.id, formData);
                setSuccessMessage('✅ Collection center updated successfully.');
                // Update the centers list
                setCenters(prev => prev.map(c => 
                    c.id === response.id ? response : c
                ));
            } else {
                // Create new center
                response = await centerService.createCenter(formData);
                setSuccessMessage('✅ Collection center added successfully.');
                setCenters(prev => [...prev, response]);
            }
            
            // Reset form
            resetForm();
        } catch (err) {
            let errorMsg = '❌ Failed to ' + (editingCenter ? 'update' : 'create') + ' collection center.';
            if (err.response?.status === 409) {
                errorMsg = '❌ A center with this name already exists.';
            } else if (err.response?.status === 400) {
                errorMsg = '❌ Invalid data. Please check your inputs.';
            }
            setError(errorMsg);
            console.error('Submit error:', err);
        }
    };

    // Reset form
    const resetForm = () => {
        setFormData({
            name: '',
            district: '',
            address: '',
            phone: '',
            acceptedItemType: '',
            openingHours: ''
        });
        setFormErrors({});
        setEditingCenter(null);
        setShowForm(false);
    };

    // Edit a center
    const handleEdit = (center) => {
        setEditingCenter(center);
        setFormData({
            name: center.name,
            district: center.district,
            address: center.address,
            phone: center.phone,
            acceptedItemType: center.acceptedItemType,
            openingHours: center.openingHours
        });
        setShowForm(true);
        setFormErrors({});
        window.scrollTo({ top: 0, behavior: 'smooth' });
    };

    // Delete a center
    const handleDelete = async () => {
        if (!centerToDelete) return;
        
        try {
            await centerService.deleteCenter(centerToDelete.id);
            setSuccessMessage('✅ Collection center deleted successfully.');
            setCenters(prev => prev.filter(c => c.id !== centerToDelete.id));
            setShowDeleteModal(false);
            setCenterToDelete(null);
        } catch (err) {
            setError('❌ Failed to delete collection center.');
            console.error('Delete error:', err);
            setShowDeleteModal(false);
        }
    };

    // Cancel delete
    const cancelDelete = () => {
        setShowDeleteModal(false);
        setCenterToDelete(null);
    };

    // Filter centers based on search term
    const filteredCenters = centers.filter(center => 
        center.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        center.district.toLowerCase().includes(searchTerm.toLowerCase()) ||
        center.address.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="manage-centers-container">
            {/* Header */}
            <div className="centers-header">
                <div className="header-left">
                    <h1>📍 Collection Centers</h1>
                    <span className="center-count">{filteredCenters.length} centers</span>
                </div>
                <button 
                    className="btn-add"
                    onClick={() => {
                        setShowForm(true);
                        setEditingCenter(null);
                        setFormData({
                            name: '',
                            district: '',
                            address: '',
                            phone: '',
                            acceptedItemType: '',
                            openingHours: ''
                        });
                        setFormErrors({});
                    }}
                >
                    <span className="icon">+</span> Add Center
                </button>
            </div>

            {/* Search Bar */}
            <div className="search-container">
                <div className="search-wrapper">
                    <span className="search-icon">🔍</span>
                    <input
                        type="text"
                        placeholder="Search by name, district, or address..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        className="search-input"
                    />
                    {searchTerm && (
                        <button 
                            className="search-clear"
                            onClick={() => setSearchTerm('')}
                            aria-label="Clear search"
                        >
                            ×
                        </button>
                    )}
                </div>
            </div>

            {/* Success Message */}
            {successMessage && (
                <div className="alert success">
                    <span className="alert-icon">✅</span>
                    {successMessage}
                    <button className="alert-close" onClick={() => setSuccessMessage(null)}>×</button>
                </div>
            )}

            {/* Error Message */}
            {error && (
                <div className="alert error">
                    <span className="alert-icon">❌</span>
                    {error}
                    <button className="alert-close" onClick={() => setError(null)}>×</button>
                </div>
            )}

            {/* Create/Edit Form */}
            {showForm && (
                <div className="center-form-container">
                    <div className="center-form">
                        <div className="form-header">
                            <h2>{editingCenter ? '✏️ Edit Collection Center' : '➕ Add New Collection Center'}</h2>
                            <button className="form-close" onClick={resetForm}>×</button>
                        </div>
                        <form onSubmit={handleSubmit}>
                            <div className="form-row">
                                <div className="form-group">
                                    <label htmlFor="name">Center Name <span className="required">*</span></label>
                                    <input
                                        type="text"
                                        id="name"
                                        name="name"
                                        value={formData.name}
                                        onChange={handleInputChange}
                                        className={formErrors.name ? 'error' : ''}
                                        placeholder="Enter center name"
                                    />
                                    {formErrors.name && <span className="error-message">{formErrors.name}</span>}
                                </div>

                                <div className="form-group">
                                    <label htmlFor="district">District <span className="required">*</span></label>
                                    <input
                                        type="text"
                                        id="district"
                                        name="district"
                                        value={formData.district}
                                        onChange={handleInputChange}
                                        className={formErrors.district ? 'error' : ''}
                                        placeholder="Enter district"
                                    />
                                    {formErrors.district && <span className="error-message">{formErrors.district}</span>}
                                </div>
                            </div>

                            <div className="form-group">
                                <label htmlFor="address">Address <span className="required">*</span></label>
                                <input
                                    type="text"
                                    id="address"
                                    name="address"
                                    value={formData.address}
                                    onChange={handleInputChange}
                                    className={formErrors.address ? 'error' : ''}
                                    placeholder="Enter full address"
                                />
                                {formErrors.address && <span className="error-message">{formErrors.address}</span>}
                            </div>

                            <div className="form-row">
                                <div className="form-group">
                                    <label htmlFor="phone">Phone Number <span className="required">*</span></label>
                                    <input
                                        type="tel"
                                        id="phone"
                                        name="phone"
                                        value={formData.phone}
                                        onChange={handleInputChange}
                                        className={formErrors.phone ? 'error' : ''}
                                        placeholder="e.g., 0112345678"
                                    />
                                    {formErrors.phone && <span className="error-message">{formErrors.phone}</span>}
                                </div>

                                <div className="form-group">
                                    <label htmlFor="acceptedItemType">Accepted Item Types <span className="required">*</span></label>
                                    <input
                                        type="text"
                                        id="acceptedItemType"
                                        name="acceptedItemType"
                                        value={formData.acceptedItemType}
                                        onChange={handleInputChange}
                                        className={formErrors.acceptedItemType ? 'error' : ''}
                                        placeholder="e.g., Batteries, Electronics"
                                    />
                                    {formErrors.acceptedItemType && <span className="error-message">{formErrors.acceptedItemType}</span>}
                                </div>
                            </div>

                            <div className="form-group">
                                <label htmlFor="openingHours">Opening Hours <span className="required">*</span></label>
                                <input
                                    type="text"
                                    id="openingHours"
                                    name="openingHours"
                                    value={formData.openingHours}
                                    onChange={handleInputChange}
                                    className={formErrors.openingHours ? 'error' : ''}
                                    placeholder="e.g., 8:00 AM - 5:00 PM"
                                />
                                {formErrors.openingHours && <span className="error-message">{formErrors.openingHours}</span>}
                            </div>

                            <div className="form-actions">
                                <button type="submit" className="btn-save">
                                    {editingCenter ? '💾 Update Center' : '💾 Save Center'}
                                </button>
                                <button type="button" className="btn-cancel" onClick={resetForm}>
                                    Cancel
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {/* Centers List */}
            <div className="centers-list">
                {loading ? (
                    <div className="loading-state">
                        <div className="spinner"></div>
                        <p>Loading collection centers...</p>
                    </div>
                ) : filteredCenters.length === 0 ? (
                    <div className="empty-state">
                        <div className="empty-icon">📭</div>
                        <p>{searchTerm ? 'No centers match your search.' : 'No collection centers available.'}</p>
                        {searchTerm && (
                            <button onClick={() => setSearchTerm('')} className="btn-clear-search">
                                Clear Search
                            </button>
                        )}
                        {!searchTerm && (
                            <button onClick={() => {
                                setShowForm(true);
                                setEditingCenter(null);
                                setFormData({
                                    name: '',
                                    district: '',
                                    address: '',
                                    phone: '',
                                    acceptedItemType: '',
                                    openingHours: ''
                                });
                            }} className="btn-add-small">
                                + Add Your First Center
                            </button>
                        )}
                    </div>
                ) : (
                    filteredCenters.map(center => (
                        <div key={center.id} className="center-card">
                            <div className="center-info">
                                <div className="center-header">
                                    <h3>{center.name}</h3>
                                    <span className="center-id">ID: #{center.id}</span>
                                </div>
                                <div className="center-details">
                                    <div className="detail-item">
                                        <span className="label">📍 District</span>
                                        <span className="value">{center.district}</span>
                                    </div>
                                    <div className="detail-item">
                                        <span className="label">🏠 Address</span>
                                        <span className="value">{center.address}</span>
                                    </div>
                                    <div className="detail-item">
                                        <span className="label">📞 Phone</span>
                                        <span className="value">{center.phone}</span>
                                    </div>
                                    <div className="detail-item">
                                        <span className="label">♻️ Accepts</span>
                                        <span className="value">{center.acceptedItemType}</span>
                                    </div>
                                    <div className="detail-item full-width">
                                        <span className="label">🕐 Hours</span>
                                        <span className="value">{center.openingHours}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="center-actions">
                                <button 
                                    className="btn-edit"
                                    onClick={() => handleEdit(center)}
                                    title="Edit center"
                                >
                                    ✏️ Edit
                                </button>
                                <button 
                                    className="btn-delete"
                                    onClick={() => {
                                        setCenterToDelete(center);
                                        setShowDeleteModal(true);
                                    }}
                                    title="Delete center"
                                >
                                    🗑️ Delete
                                </button>
                            </div>
                        </div>
                    ))
                )}
            </div>

            {/* Delete Confirmation Modal */}
            {showDeleteModal && centerToDelete && (
                <div className="modal-overlay" onClick={cancelDelete}>
                    <div className="modal" onClick={(e) => e.stopPropagation()}>
                        <div className="modal-header">
                            <h3>🗑️ Confirm Delete</h3>
                            <button className="modal-close" onClick={cancelDelete}>×</button>
                        </div>
                        <div className="modal-body">
                            <p>Are you sure you want to delete the following collection center?</p>
                            <div className="modal-center-name">{centerToDelete.name}</div>
                            <div className="modal-center-details">
                                <span>{centerToDelete.district}</span>
                                <span>•</span>
                                <span>{centerToDelete.address}</span>
                            </div>
                            <div className="modal-warning">
                                ⚠️ This action cannot be undone.
                            </div>
                        </div>
                        <div className="modal-actions">
                            <button className="btn-cancel" onClick={cancelDelete}>
                                Cancel
                            </button>
                            <button className="btn-delete-confirm" onClick={handleDelete}>
                                Yes, Delete
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ManageCenters;