import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function CustomerRegister() {
  const [form, setForm] = useState({
    name: "",
    phoneNumber: "",
    email: "",
    password: "",
    address: "",
  });
  const navigate = useNavigate();
  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    axios
      .post("https://localhost:7030/api/user/register", form)
      .then(() => {
        alert("Registered successfully");
        navigate("/login");
      })
      .catch((err) => console.error(err));
  };

  return (
    <div
      className="d-flex justify-content-center align-items-center min-vh-100"
      style={{ backgroundColor: "#e9f5e9" }} // light tea-green background
    >
      <div
        className="card shadow-sm"
        style={{ maxWidth: "480px", width: "100%" }}
      >
        <div className="card-body p-4">
          <h2
            className="card-title mb-3 text-center"
            style={{ color: "#2f5d27", fontFamily: "'Georgia', serif" }}
          >
            Tea Management System
          </h2>
          <h4 className="mb-4 text-center text-success">
            Customer Registration
          </h4>

          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label
                htmlFor="name"
                className="form-label fw-semibold text-success"
              >
                Name
              </label>
              <input
                type="text"
                id="name"
                name="name"
                value={form.name}
                onChange={handleChange}
                className="form-control"
                placeholder="Your full name"
                required
              />
            </div>

            <div className="mb-3">
              <label
                htmlFor="phoneNumber"
                className="form-label fw-semibold text-success"
              >
                Phone Number
              </label>
              <input
                type="tel"
                id="phoneNumber"
                name="phoneNumber"
                value={form.phoneNumber}
                onChange={handleChange}
                className="form-control"
                placeholder="07X XXX XXXX"
                required
              />
            </div>

            <div className="mb-3">
              <label
                htmlFor="email"
                className="form-label fw-semibold text-success"
              >
                Email
              </label>
              <input
                type="email"
                id="email"
                name="email"
                value={form.email}
                onChange={handleChange}
                className="form-control"
                placeholder="you@example.com"
                required
              />
            </div>

            <div className="mb-3">
              <label
                htmlFor="password"
                className="form-label fw-semibold text-success"
              >
                Password
              </label>
              <input
                type="password"
                id="password"
                name="password"
                value={form.password}
                onChange={handleChange}
                className="form-control"
                placeholder="Choose a secure password"
                required
              />
            </div>

            <div className="mb-4">
              <label
                htmlFor="address"
                className="form-label fw-semibold text-success"
              >
                Address
              </label>
              <input
                type="text"
                id="address"
                name="address"
                value={form.address}
                onChange={handleChange}
                className="form-control"
                placeholder="Your address"
                required
              />
            </div>

            <button
              type="submit"
              className="btn btn-success w-100 fw-semibold"
              style={{ backgroundColor: "#2f5d27", borderColor: "#2f5d27" }}
            >
              Register
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}

export default CustomerRegister;
