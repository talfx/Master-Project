import React, { useState } from 'react';
import axios from 'axios';

function AdminView() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState(null);
  const [error, setError] = useState('');

  const handleExecuteQuery = async () => {
    const token = localStorage.getItem('token');
    setError('');
    setResults(null);

    try {
      const response = await axios.post('http://localhost:5227/api/Admin/query', 
        { query },
        { headers: { 'Authorization': `Bearer ${token}` } }
      );
      setResults(response.data);
    } catch (err) {
      setError(err.response?.data?.message || 'Query execution failed');
    }
  };

  return (
    <div>
      <h3>Admin: Custom Database Query</h3>
      <div style={{ marginBottom: '20px' }}>
        <textarea
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Enter SQL query (e.g., SELECT * FROM customers LIMIT 10)"
          style={{ width: '100%', height: '100px', padding: '10px', fontFamily: 'monospace' }}
        />
        <button 
          onClick={handleExecuteQuery}
          style={{ marginTop: '10px', padding: '10px 20px', cursor: 'pointer' }}
        >
          Execute Query
        </button>
      </div>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {results && results.length > 0 && (
        <div style={{ overflowX: 'auto' }}>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
            <thead>
              <tr style={{ borderBottom: '2px solid #ddd' }}>
                {Object.keys(results[0]).map(key => (
                  <th key={key} style={{ padding: '10px', textAlign: 'left' }}>{key}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {results.map((row, idx) => (
                <tr key={idx} style={{ borderBottom: '1px solid #ddd' }}>
                  {Object.values(row).map((val, i) => (
                    <td key={i} style={{ padding: '10px' }}>{String(val)}</td>
                  ))}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {results && results.length === 0 && <p>Query returned no results.</p>}
    </div>
  );
}

export default AdminView;