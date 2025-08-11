import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function Login() {
  const [form, setForm] = useState({ email: "", password: "" });
  const [selectedRole, setSelectedRole] = useState("customer");
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    // API endpoint mapping
    const endpointMap = {
      customer: "https://localhost:7030/api/user/login",
      seller: "https://localhost:7030/api/seller/login",
      manufacturer: "https://localhost:7030/api/manufacturer/login",
    };
    // Route mapping
    const routeMap = {
      customer: "/customer",
      seller: "/seller",
      manufacturer: "/manufacturer",
    };

    const endpoint = endpointMap[selectedRole];
    axios
      .post(endpoint, form)
      .then(() => {
        navigate(routeMap[selectedRole]);
      })
      .catch((error) => {
        console.error("Login failed:", error);
        alert("Login failed. Please check your credentials.");
      });
  };

  return (
    <div
      className="d-flex justify-content-center align-items-center min-vh-100"
      style={{ backgroundColor: "#f0f8ff" }}
    >
      <div
        className="card shadow p-4"
        style={{ maxWidth: "400px", width: "100%" }}
      >
        <h2 className="text-center mb-4" style={{ color: "#0d6efd" }}>
          Login
        </h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="roleSelect" className="form-label">
              Select Role:
            </label>
            <select
              id="roleSelect"
              value={selectedRole}
              onChange={(e) => setSelectedRole(e.target.value)}
              className="form-select"
            >
              <option value="customer">Customer</option>
              <option value="seller">Seller</option>
              <option value="manufacturer">Manufacturer</option>
            </select>
          </div>

          <div className="mb-3">
            <label htmlFor="email" className="form-label">
              Email:
            </label>
            <input
              id="email"
              type="email"
              name="email"
              value={form.email}
              onChange={handleChange}
              className="form-control"
              placeholder="Enter your email"
              required
            />
          </div>

          <div className="mb-4">
            <label htmlFor="password" className="form-label">
              Password:
            </label>
            <input
              id="password"
              type="password"
              name="password"
              value={form.password}
              onChange={handleChange}
              className="form-control"
              placeholder="Enter your password"
              required
            />
          </div>

          <button
            type="submit"
            className="btn btn-primary w-100"
            style={{ backgroundColor: "#0d6efd", borderColor: "#0d6efd" }}
          >
            Login
          </button>
        </form>
      </div>
    </div>
  );
}

export default Login;
