import React, { useEffect, useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";

function CustomerDashboard() {
  const [products, setProducts] = useState([]);
  const [orders, setOrders] = useState([]);
  const [formData, setFormData] = useState({
    productId: "",
    quantitySold: 1,
    paymentType: "Full Payment",
  });
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  const customerId = localStorage.getItem("customerId") || "1";

  useEffect(() => {
    setLoading(true);

    axios
      .get("https://localhost:7030/api/product/get-all")
      .then((res) => setProducts(res.data))
      .catch((err) =>
        setError(
          `Failed to fetch products: ${
            err.response?.data?.message || err.message
          }`
        )
      );

    axios
      .get("https://localhost:7030/api/Order/all")
      .then((res) => {
        const filteredOrders = res.data.filter(
          (order) => String(order.customerId) === String(customerId)
        );
        setOrders(filteredOrders);
      })
      .catch((err) =>
        setError(
          `Failed to fetch orders: ${
            err.response?.data?.message || err.message
          }`
        )
      )
      .finally(() => setLoading(false));
  }, [customerId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    const selectedProduct = products.find(
      (p) => p.productId === parseInt(formData.productId)
    );
    if (!selectedProduct) {
      setError("Please select a valid product.");
      setLoading(false);
      return;
    }

    const quantitySold = parseInt(formData.quantitySold);
    if (isNaN(quantitySold) || quantitySold <= 0) {
      setError("Quantity must be a positive number.");
      setLoading(false);
      return;
    }

    if (quantitySold > selectedProduct.fullQuantity) {
      setError(
        `Quantity cannot exceed available stock (${selectedProduct.fullQuantity}).`
      );
      setLoading(false);
      return;
    }

    if (!["Full Payment", "Lend"].includes(formData.paymentType)) {
      setError("Invalid payment type. Choose 'Full Payment' or 'Lend'.");
      setLoading(false);
      return;
    }

    const customerIdNum = parseInt(customerId);
    if (isNaN(customerIdNum) || customerIdNum <= 0) {
      setError("Invalid customer ID.");
      setLoading(false);
      return;
    }

    const payload = {
      CustomerId: customerIdNum,
      ProductId: parseInt(formData.productId),
      SellingPrice: Number(selectedProduct.sellingPrice),
      QuantitySold: quantitySold,
      PaymentType: formData.paymentType,
      paymentStatus:
        formData.paymentType === "Full Payment" ? "Paid" : "Pending",
      SellingDate: new Date().toISOString().replace("Z", ""),
    };

    axios
      .post("https://localhost:7030/api/Order", payload)
      .then(() => {
        alert("Order request submitted successfully!");
        setFormData({
          productId: "",
          quantitySold: 1,
          paymentType: "Full Payment",
        });
      })
      .catch((err) =>
        setError(
          `Failed to submit order: ${JSON.stringify(
            err.response?.data || err.message
          )}`
        )
      )
      .finally(() => setLoading(false));
  };

  return (
    <div className="container py-4">
      {error && <div className="alert alert-danger">{error}</div>}
      {loading && <div className="alert alert-info">Loading...</div>}

      <h2 className="mb-4">👀 View Tea Products</h2>
      {products.length === 0 ? (
        <p>No products available.</p>
      ) : (
        <div className="table-responsive mb-4">
          <table className="table table-bordered table-hover table-striped">
            <thead className="table-dark">
              <tr>
                <th>Type</th>
                <th>Selling Price</th>
                <th>Available Quantity</th>
              </tr>
            </thead>
            <tbody>
              {products.map((p) => (
                <tr key={p.productId}>
                  <td>{p.productType}</td>
                  <td>Rs. {Number(p.sellingPrice).toFixed(2)}</td>
                  <td>{p.fullQuantity}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <h2 className="mb-4">📝 Request an Order</h2>
      <form onSubmit={handleSubmit} className="mb-5">
        <div className="mb-3">
          <label className="form-label">Product Type</label>
          <select
            name="productId"
            value={formData.productId}
            onChange={handleChange}
            className="form-select"
            required
          >
            <option value="">Select</option>
            {products.map((p) => (
              <option key={p.productId} value={p.productId}>
                {p.productType}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-3">
          <label className="form-label">Quantity</label>
          <input
            type="number"
            name="quantitySold"
            min="1"
            value={formData.quantitySold}
            onChange={handleChange}
            className="form-control"
            required
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Payment Type</label>
          <select
            name="paymentType"
            value={formData.paymentType}
            onChange={handleChange}
            className="form-select"
            required
          >
            <option value="Full Payment">Full Payment</option>
            <option value="Lend">Lend</option>
          </select>
        </div>

        <button type="submit" className="btn btn-success" disabled={loading}>
          {loading ? "Submitting..." : "Submit Order"}
        </button>
      </form>

      <h2 className="mb-4">My Orders</h2>
      {orders.length === 0 ? (
        <p>No orders found.</p>
      ) : (
        <div className="table-responsive">
          <table className="table table-bordered table-striped table-hover">
            <thead className="table-dark">
              <tr>
                <th>Order ID</th>
                <th>Product ID</th>
                <th>Seller ID</th>
                <th>Quantity</th>
                <th>Selling Price</th>
                <th>Payment Type</th>
                <th>Money Given Date</th>
                <th>Payment Status</th>
                <th>Selling Date</th>
                <th>Approval Status</th>
              </tr>
            </thead>
            <tbody>
              {orders.map((order) => (
                <tr key={order.orderId}>
                  <td>{order.orderId}</td>
                  <td>{order.productId}</td>
                  <td>{order.sellerId}</td>
                  <td>{order.quantitySold}</td>
                  <td>Rs. {Number(order.sellingPrice).toFixed(2)}</td>
                  <td>{order.paymentType}</td>
                  <td>
                    {order.moneyGivenDate
                      ? new Date(order.moneyGivenDate).toLocaleDateString()
                      : "N/A"}
                  </td>
                  <td>{order.paymentStatus}</td>
                  <td>{new Date(order.sellingDate).toLocaleString()}</td>
                  <td>{order.sellerApprovalStatus}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export default CustomerDashboard;
