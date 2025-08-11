import React, { useEffect, useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";

function ManufacturerDashboard() {
  const [productForm, setProductForm] = useState({
    productType: "250g",
    fullQuantity: "",
    costPrice: "",
    sellingPrice: "",
    manufacturerId: 1, // Replace with dynamic ID if needed
  });
  const [requests, setRequests] = useState([]);
  const [allProducts, setAllProducts] = useState([]);

  useEffect(() => {
    fetchAll();
  }, []);

  const fetchAll = () => {
    axios
      .get("https://localhost:7030/api/request/get-all")
      .then((res) => setRequests(res.data))
      .catch((err) => console.error(err));

    axios
      .get("https://localhost:7030/api/product/get-all")
      .then((res) => setAllProducts(res.data))
      .catch((err) => console.error(err));
  };

  const handleProductChange = (e) => {
    const { name, value } = e.target;
    setProductForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleAddProduct = (e) => {
    e.preventDefault();
    axios
      .post("https://localhost:7030/api/manufacturer/addProduct", productForm)
      .then(() => {
        alert("Product added");
        fetchAll();
      })
      .catch((err) => {
        console.error(err);
        alert("Failed to add product.");
      });
  };

  const handleApprove = (id) => {
    axios
      .post(`https://localhost:7030/api/request/approve/${id}`)
      .then(() => fetchAll())
      .catch((err) => console.error(err));
  };

  const handleReject = (id) => {
    axios
      .post(`https://localhost:7030/api/request/reject/${id}`)
      .then(() => fetchAll())
      .catch((err) => console.error(err));
  };

  return (
    <div
      className="container-fluid py-4"
      style={{ backgroundColor: "#f8f9fa", minHeight: "100vh" }}
    >
      <div className="container">
        {/* Add Product Form */}
        <div className="card shadow-sm mb-4">
          <div className="card-header bg-success text-white">
            🧾 Add Product to Main Stock
          </div>
          <div className="card-body">
            <form onSubmit={handleAddProduct}>
              <div className="mb-3">
                <label className="form-label">Type</label>
                <select
                  name="productType"
                  value={productForm.productType}
                  onChange={handleProductChange}
                  className="form-select"
                >
                  <option value="250g">250g</option>
                  <option value="100g">100g</option>
                </select>
              </div>
              <div className="mb-3">
                <label className="form-label">Full Quantity</label>
                <input
                  type="number"
                  name="fullQuantity"
                  value={productForm.fullQuantity}
                  onChange={handleProductChange}
                  className="form-control"
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">Cost Price</label>
                <input
                  type="number"
                  name="costPrice"
                  value={productForm.costPrice}
                  onChange={handleProductChange}
                  className="form-control"
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">Selling Price</label>
                <input
                  type="number"
                  name="sellingPrice"
                  value={productForm.sellingPrice}
                  onChange={handleProductChange}
                  className="form-control"
                  required
                />
              </div>
              <button type="submit" className="btn btn-success">
                Add Product
              </button>
            </form>
          </div>
        </div>

        {/* Pending Product Requests */}
        <div className="card shadow-sm mb-4">
          <div className="card-header bg-primary text-white">
            ✅ Pending Product Requests
          </div>
          <div className="card-body p-0">
            <table className="table table-hover table-striped mb-0">
              <thead className="table-light">
                <tr>
                  <th>Request ID</th>
                  <th>Seller ID</th>
                  <th>Message</th>
                  <th>Product Type</th>
                  <th>Date</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {requests.map((r) => (
                  <tr key={r.requestId}>
                    <td>{r.requestId}</td>
                    <td>{r.sellerId}</td>
                    <td>{r.requestMessage}</td>
                    <td>{r.productType}</td>
                    <td>{new Date(r.requestDate).toLocaleDateString()}</td>
                    <td>{r.status}</td>
                    <td>
                      <button
                        onClick={() => handleApprove(r.requestId)}
                        className="btn btn-sm btn-outline-primary me-2"
                      >
                        Approve
                      </button>
                      <button
                        onClick={() => handleReject(r.requestId)}
                        className="btn btn-sm btn-outline-danger"
                      >
                        Reject
                      </button>
                    </td>
                  </tr>
                ))}
                {requests.length === 0 && (
                  <tr>
                    <td colSpan="7" className="text-center py-3">
                      No pending requests.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>

        {/* All Products */}
        <div className="card shadow-sm">
          <div className="card-header bg-dark text-white">📦 All Products</div>
          <div className="card-body p-0">
            <table className="table table-hover table-striped mb-0">
              <thead className="table-light">
                <tr>
                  <th>Type</th>
                  <th>Quantity</th>
                  <th>Cost Price</th>
                  <th>Selling Price</th>
                </tr>
              </thead>
              <tbody>
                {allProducts.map((p, idx) => (
                  <tr key={idx}>
                    <td>{p.productType}</td>
                    <td>{p.fullQuantity}</td>
                    <td>Rs. {p.costPrice}</td>
                    <td>Rs. {p.sellingPrice}</td>
                  </tr>
                ))}
                {allProducts.length === 0 && (
                  <tr>
                    <td colSpan="4" className="text-center py-3">
                      No products available.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ManufacturerDashboard;
