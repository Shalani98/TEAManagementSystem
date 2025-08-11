import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";

import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
} from "react-router-dom";
import CustomerRegister from "./pages/CustomerRegister";
import Login from "./pages/Login";
import ManufacturerDashboard from "./pages/ManufacturerDashboard";
import SellerDashboard from "./pages/SellerDashboard";
import CustomerDashboard from "./pages/CustomerDashboard";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/register" element={<CustomerRegister />} />
        <Route path="/login" element={<Login />} />
        <Route path="/manufacturer" element={<ManufacturerDashboard />} />
        <Route path="/seller" element={<SellerDashboard />} />
        <Route path="/customer" element={<CustomerDashboard />} />
      </Routes>
    </Router>
  );
}

export default App;
