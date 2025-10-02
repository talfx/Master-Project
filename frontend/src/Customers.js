import React, { useState, useEffect } from 'react';
import axios from 'axios';

function Customers() {
  const [customers, setCustomers] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchCustomers = async () => {
      const token = localStorage.getItem('token');
      
      try {
        const response = await axios.get('http://localhost:5227/api/Customers', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        setCustomers(response.data);
      } catch (err) {
        setError('Failed to fetch customers: ' + (err.response?.data?.message || 'Unknown error'));
      }
    };

    fetchCustomers();
  }, []);

  return (
    <div style={{ padding: '20px' }}>
      <h2>Customers</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ borderBottom: '2px solid #ddd' }}>
            <th style={{ padding: '10px', textAlign: 'left' }}>ID</th>
            <th style={{ padding: '10px', textAlign: 'left' }}>Name</th>
            <th style={{ padding: '10px', textAlign: 'left' }}>Email</th>
            <th style={{ padding: '10px', textAlign: 'left' }}>City</th>
          </tr>
        </thead>
        <tbody>
          {customers.map(customer => (
            <tr key={customer.customerId} style={{ borderBottom: '1px solid #ddd' }}>
              <td style={{ padding: '10px' }}>{customer.customerId}</td>
              <td style={{ padding: '10px' }}>{customer.firstName} {customer.lastName}</td>
              <td style={{ padding: '10px' }}>{customer.email}</td>
              <td style={{ padding: '10px' }}>{customer.city}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Customers;