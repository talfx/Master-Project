import React, { useState, useEffect } from 'react';
import axios from 'axios';

function CustomerView({ userId }) {
  const [customer, setCustomer] = useState(null);
  const [orders, setOrders] = useState([]);

  useEffect(() => {
    const fetchCustomerData = async () => {
      const token = localStorage.getItem('token');
      
      try {
        // Fetch all customers and find the one linked to this user
        const customersResponse = await axios.get('http://localhost:5227/api/Customers', {
          headers: { 'Authorization': `Bearer ${token}` }
        });
        
        const myCustomer = customersResponse.data.find(c => c.userId === parseInt(userId));
        setCustomer(myCustomer);
        
        if (myCustomer) {
          // Fetch all orders and filter by customer
          const ordersResponse = await axios.get('http://localhost:5227/api/Orders', {
            headers: { 'Authorization': `Bearer ${token}` }
          });
          
          const myOrders = ordersResponse.data.filter(o => o.customerId === myCustomer.customerId);
          setOrders(myOrders);
        }
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchCustomerData();
  }, [userId]);

  if (!customer) return <div>Loading...</div>;

  return (
    <div>
      <h3>My Profile</h3>
      <div style={{ marginBottom: '30px', padding: '15px', backgroundColor: '#f5f5f5', borderRadius: '5px' }}>
        <p><strong>Name:</strong> {customer.firstName} {customer.lastName}</p>
        <p><strong>Email:</strong> {customer.email}</p>
        <p><strong>Phone:</strong> {customer.phone}</p>
        <p><strong>Address:</strong> {customer.address}, {customer.city}, {customer.country}</p>
      </div>

      <h3>My Orders</h3>
      {orders.length === 0 ? (
        <p>No orders yet.</p>
      ) : (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #ddd' }}>
              <th style={{ padding: '10px', textAlign: 'left' }}>Order ID</th>
              <th style={{ padding: '10px', textAlign: 'left' }}>Date</th>
              <th style={{ padding: '10px', textAlign: 'left' }}>Status</th>
              <th style={{ padding: '10px', textAlign: 'left' }}>Total</th>
            </tr>
          </thead>
          <tbody>
            {orders.map(order => (
              <tr key={order.orderId} style={{ borderBottom: '1px solid #ddd' }}>
                <td style={{ padding: '10px' }}>{order.orderId}</td>
                <td style={{ padding: '10px' }}>{new Date(order.orderDate).toLocaleDateString()}</td>
                <td style={{ padding: '10px' }}>{order.status}</td>
                <td style={{ padding: '10px' }}>${order.totalAmount}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default CustomerView;