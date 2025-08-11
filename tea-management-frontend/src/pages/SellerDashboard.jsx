import React, { useState, useEffect } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";

function SellerDashboard() {
  const [requestText, setRequestText] = useState("");
  const [productType, setProductType] = useState("");
  const [orderId, setOrderId] = useState("");
  const [approvalStatus, setApprovalStatus] = useState("Approved");
  const [sellerProducts, setSellerProducts] = useState([]);
  const [orders, setOrders] = useState([]);

  const sellerId = 1; // Replace with dynamic seller ID

  useEffect(() => {
    fetchSellerProducts();
    fetchOrders();
  }, []);

  const fetchSellerProducts = async () => {
    try {
      const res = await axios.get(
        `https://localhost:7030/api/seller/products/${sellerId}`
      );
      setSellerProducts(res.data);
    } catch (error) {
      console.error("Error fetching seller products:", error);
    }
  };

  const fetchOrders = async () => {
    try {
      const res = await axios.get("https://localhost:7030/api/Order/all");
      setOrders(res.data);
    } catch (error) {
      console.error("Error fetching orders:", error);
    }
  };

  const handleRequestSubmit = async (e) => {
    e.preventDefault();
    const requestPayload = {
      requestId: 0,
      sellerId: sellerId,
      seller: {
        id: sellerId,
        email: "sample@gmail.com",
        password: "Sel@1998*",
      },
      requestMessage: requestText,
      productType: productType,
      requestDate: new Date().toISOString(),
      status: "Pending",
    };

    try {
      await axios.post(
        "https://localhost:7030/api/request/create",
        requestPayload
      );
      alert("Product request created successfully!");
      setRequestText("");
      setProductType("");
    } catch (error) {
      console.error("Error submitting request:", error);
      alert("Failed to submit product request.");
    }
  };

  const handleOrderApproval = async (
    e,
    orderIdParam = null,
    statusParam = null
  ) => {
    if (e) e.preventDefault();
    const selectedOrderId = orderIdParam || parseInt(orderId);
    const selectedStatus = statusParam || approvalStatus;

    const order = orders.find((o) => o.orderId === selectedOrderId);
    if (!order) {
      alert("Order not found.");
      return;
    }

    const payload = {
      orderId: selectedOrderId,
      sellerApprovalStatus: selectedStatus,
      productId: order.productId,
      quantitySold: order.quantitySold,
    };

    try {
      await axios.put(
        `https://localhost:7030/api/Order/approve?sellerId=${sellerId}`,
        payload,
        { headers: { "Content-Type": "application/json" } }
      );
      alert("Order approval updated.");
      fetchOrders();
    } catch (error) {
      console.error("Error updating order:", error);
      alert("Failed to update order.");
    }
  };

  return (
    <div className="container py-4">
      {/* Submit Product Request */}
      <h2 className="mb-3">Submit Product Request</h2>
      <form onSubmit={handleRequestSubmit} className="mb-4">
        <div className="mb-3">
          <label className="form-label">Product Type</label>
          <input
            type="text"
            className="form-control"
            value={productType}
            onChange={(e) => setProductType(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Request Message</label>
          <textarea
            className="form-control"
            value={requestText}
            onChange={(e) => setRequestText(e.target.value)}
            required
          ></textarea>
        </div>
        <button type="submit" className="btn btn-success">
          Submit Request
        </button>
      </form>

      <hr />

      {/* Manual Order Approval */}
      <h2 className="mb-3">Approve Order (Manual Entry)</h2>
      <form onSubmit={handleOrderApproval} className="mb-4">
        <div className="mb-3">
          <label className="form-label">Order ID</label>
          <input
            type="number"
            className="form-control"
            value={orderId}
            onChange={(e) => setOrderId(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Approval Status</label>
          <select
            className="form-select"
            value={approvalStatus}
            onChange={(e) => setApprovalStatus(e.target.value)}
          >
            <option value="Approved">Approved</option>
            <option value="Rejected">Rejected</option>
          </select>
        </div>
        <button type="submit" className="btn btn-primary">
          Submit Approval
        </button>
      </form>

      <hr />

      {/* Orders Table */}
      <h2 className="mb-3">All Orders</h2>
      {orders.length === 0 ? (
        <p>No orders found.</p>
      ) : (
        <div className="table-responsive mb-4">
          <table className="table table-bordered table-striped table-hover">
            <thead className="table-dark">
              <tr>
                <th>ID</th>
                <th>Product ID</th>
                <th>Customer ID</th>
                <th>Selling Price</th>
                <th>Qty</th>
                <th>Payment Type</th>
                <th>Status</th>
                <th>Approval</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {orders.map((order) => (
                <tr key={order.orderId}>
                  <td>{order.orderId}</td>
                  <td>{order.productId}</td>
                  <td>{order.customerId}</td>
                  <td>Rs. {Number(order.sellingPrice).toFixed(2)}</td>
                  <td>{order.quantitySold}</td>
                  <td>{order.paymentType}</td>
                  <td>
                    <span
                      className={`badge ${
                        order.paymentStatus === "Paid"
                          ? "bg-success"
                          : "bg-warning text-dark"
                      }`}
                    >
                      {order.paymentStatus}
                    </span>
                  </td>
                  <td>{order.sellerApprovalStatus}</td>
                  <td>
                    <button
                      className="btn btn-sm btn-success me-2"
                      onClick={() =>
                        handleOrderApproval(null, order.orderId, "Approved")
                      }
                      disabled={order.sellerApprovalStatus !== "Pending"}
                    >
                      Approve
                    </button>
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() =>
                        handleOrderApproval(null, order.orderId, "Rejected")
                      }
                      disabled={order.sellerApprovalStatus !== "Pending"}
                    >
                      Reject
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Seller Product Stock */}
      <h2 className="mb-3">Seller Product Stock</h2>
      {sellerProducts.length === 0 ? (
        <p>No products found.</p>
      ) : (
        <div className="table-responsive">
          <table className="table table-bordered table-striped table-hover">
            <thead className="table-dark">
              <tr>
                <th>Seller Product ID</th>
                <th>Product ID</th>
                <th>Quantity</th>
                <th>Quantity Sold</th>
                <th>Stock Balance</th>
                <th>Cost Price</th>
                <th>Selling Price</th>
                <th>Profit</th>
              </tr>
            </thead>
            <tbody>
              {sellerProducts.map((prod) => (
                <tr key={prod.sellerProductId}>
                  <td>{prod.sellerProductId}</td>
                  <td>{prod.productId}</td>
                  <td>{prod.quantity}</td>
                  <td>{prod.quantitySold}</td>
                  <td>{prod.stockBalance}</td>
                  <td>Rs. {Number(prod.costPrice).toFixed(2)}</td>
                  <td>Rs. {Number(prod.sellingPrice).toFixed(2)}</td>
                  <td>Rs. {Number(prod.profit).toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export default SellerDashboard;
