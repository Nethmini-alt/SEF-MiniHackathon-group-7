/* eslint-disable no-unused-vars */
import React from 'react';
import './App.css';
import ManageCenters from './pages/admin/ManageCenters';

function App() {
  return (
    <div className="App">
      <header className="app-header">
        <div className="header-content">
          <h1>♻️ E-Waste Management System</h1>
          <span className="admin-badge">Admin Dashboard</span>
        </div>
      </header>
      <main className="app-main">
        <ManageCenters />
      </main>
      <footer className="app-footer">
        <p>© 2024 E-Waste Management System. All rights reserved.</p>
      </footer>
    </div>
  );
}

export default App;