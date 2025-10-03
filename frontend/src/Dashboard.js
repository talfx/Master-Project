import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import CustomerView from './CustomerView';
import AdminView from './AdminView';


function Dashboard() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/');
      return;
    }

    // Decode token to get user info
    const payload = JSON.parse(atob(token.split('.')[1]));
    setUser({
      userId: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
      username: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    });
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  if (!user) return <div>Loading...</div>;

  return (
    <div style={{ padding: '20px' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '30px', borderBottom: '2px solid #ddd', paddingBottom: '10px' }}>
        <div>
          <h2>Welcome, {user.username}!</h2>
          <span style={{ padding: '5px 10px', backgroundColor: '#007bff', color: 'white', borderRadius: '5px' }}>
            {user.role}
          </span>
        </div>
        <button onClick={handleLogout} style={{ padding: '10px 20px', cursor: 'pointer' }}>
          Logout
        </button>
      </div>
      
      <div>
        {user.role === 'Admin' && <AdminView />}
        {user.role === 'Customer' && <CustomerView userId={user.userId} />}
        {user.role === 'Employee' && <EmployeeView />}
      </div>
    </div>
  );
}

function AdminView() {
  return <div><h3>Admin Dashboard</h3><p>Query interface coming next...</p></div>;
}

function CustomerView({ userId }) {
  return <div><h3>Customer Dashboard</h3><p>Your orders coming next...</p></div>;
}

function EmployeeView() {
  return <div><h3>Employee Dashboard</h3><p>Employee tools...</p></div>;
}

export default Dashboard;