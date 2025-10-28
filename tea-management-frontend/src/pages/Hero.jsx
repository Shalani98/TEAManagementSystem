import React from "react";
import "../App.css"; // go up one folder to access App.css
import teaImage from "../assets/images/tea.jpg"; // go up one folder to reach assets
import { useNavigate } from "react-router-dom";
function Hero() {
  const navigate = useNavigate();

  const handleSubmit = () => {
    navigate("/register");
  };
  return (
    <div className="hero" style={{ backgroundImage: `url(${teaImage})` }}>
      <div className="hero-content">
        <h1>Welcome to Tea Management System</h1>
        <p>Manage your tea products, orders, and stock efficiently</p>
        <button className="get-started-btn" onClick={handleSubmit}>
          Get Started
        </button>
      </div>
    </div>
  );
}

export default Hero;
