import React, { useState } from 'react';
import Login from './Login';
import Customers from './Customers';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));

  return (
    <div className="App">
      {!isLoggedIn ? (
        <Login onLoginSuccess={() => setIsLoggedIn(true)} />
      ) : (
        <Customers />
      )}
    </div>
  );
}

export default App;