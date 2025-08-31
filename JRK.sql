-- Create Database
CREATE DATABASE JRK;
USE JRK;

-- 1. Customer Table
CREATE TABLE Customer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Password NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(200)
);

INSERT INTO Customer (Name, Email, Password, PhoneNumber, Address)
VALUES 
('John Doe', 'john.doe@example.com', 'password123', '0771234567', '123 Main Street'),
('Jane Smith', 'jane.smith@example.com', 'securepass', '0712345678', '456 Elm Road'),
('Nimal Perera', 'nimal.perera@example.com', 'pass789', '0729876543', '789 Lotus Ave');

SELECT * FROM Customer WHERE Id = 1;
-- 2. Seller Table
CREATE TABLE Seller (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(100),
    Password NVARCHAR(100)
);
INSERT INTO Seller (Email, Password)
VALUES ('sample@gmail.com', 'Sel@1998*');
SELECT*FROM Seller;
-- 3. Manufacturer Table
CREATE TABLE Manufacturer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(100),
    Password NVARCHAR(100)
);

INSERT INTO Manufacturer (Email, Password)
VALUES ('sample@gmail.com', 'Manu@1998*');
SELECT*FROM Manufacturer;
-- 4. Product Table (Manufacturer's Main Stock)
CREATE TABLE Product (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductType NVARCHAR(100) NOT NULL,
    CostPrice DECIMAL(10, 2) NOT NULL,
    SellingPrice DECIMAL(10, 2) NOT NULL,
    FullQuantity INT NOT NULL,
    QuantitySold INT NOT NULL DEFAULT 0,
    Profit AS (SellingPrice - CostPrice) PERSISTED,
    StockBalance AS (FullQuantity - QuantitySold) PERSISTED,
    DateAdded DATE NOT NULL DEFAULT GETDATE(),
    ManufacturerId INT NOT NULL FOREIGN KEY REFERENCES Manufacturer(Id)
);
SELECT *FROM Product
-- 5. SellerProduct Table (Each seller's selling version of a product)
CREATE TABLE SellerProduct (
    SellerProductId INT IDENTITY(1,1) PRIMARY KEY,
    SellerId INT NOT NULL FOREIGN KEY REFERENCES Seller(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
    Quantity INT NOT NULL,
    QuantitySold INT NOT NULL DEFAULT 0,
    SellingPrice DECIMAL(10, 2) NOT NULL,
    CostPrice DECIMAL(10, 2) NOT NULL,  -- Copy from Product table at insert time
    Profit AS (SellingPrice - CostPrice) PERSISTED,
    StockBalance AS (Quantity - QuantitySold) PERSISTED,
    DateAdded DATE NOT NULL DEFAULT GETDATE()
);
SELECT *FROM Product
SELECT*FROM SellerProduct
-- 6. Orders Table (Customer buying from Seller)
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Customer(Id),
    SellerId INT FOREIGN KEY REFERENCES Seller(Id),
    ProductId INT FOREIGN KEY REFERENCES Product(ProductId),
    SellingPrice DECIMAL(10, 2) NOT NULL,
    QuantitySold INT NOT NULL,
    PaymentType NVARCHAR(20) NOT NULL CHECK (PaymentType IN ('Full Payment', 'Lend')),
    MoneyGivenDate DATE NULL, -- Nullable if full payment
    PaymentStatus NVARCHAR(20) NOT NULL CHECK (PaymentStatus IN ('Paid', 'Pending')),
    SellingDate DATETIME NOT NULL DEFAULT GETDATE(),
    SellerApprovalStatus NVARCHAR(20) DEFAULT 'Pending' CHECK (SellerApprovalStatus IN ('Pending', 'Approved', 'Rejected'))
);
UPDATE Orders
SET SellerId = 1
WHERE OrderId = 6;
SELECT*FROM Orders
-- 7. Request Table (For seller restock/approval)
CREATE TABLE Request (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    SellerId INT FOREIGN KEY REFERENCES Seller(Id),
    RequestMessage NVARCHAR(300),
    ProductType NVARCHAR(100),
    RequestDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected'))
);

