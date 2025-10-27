# Tea Management System

This is a Tea Management System that facilitates product management and ordering between manufacturers, sellers, and customers.

## Features

- **Manufacturer** can add new tea products.
- **Seller** can add products from the manufacturer to their own inventory.
- **Customer** can browse and order tea packets from sellers.
- **Seller** can approve or reject customer orders.
- **Seller** can request stock from the manufacturer if inventory is low.
- **Manufacturer** can approve or reject stock requests from sellers.
- Automatic stock updates based on approvals and orders.

## Technologies Used

- **Backend:** C# (.NET Core / ASP.NET Core)
- **Frontend:** React
- **Styling:** Bootstrap CSS

## How It Works

1. Manufacturer adds tea products.
2. Sellers add products to their inventory.
3. Customers place orders for tea packets from sellers.
4. Sellers manage orders by approving or rejecting them.
5. Sellers request additional stock from the manufacturer if needed.
6. Manufacturer approves or denies stock requests.
7. Stock levels update accordingly.

## How to Run

1. Download and install Visual Studio 2022 with the **.NET desktop development** and **ASP.NET and web development** workloads.
2. Clone the repository to your local machine.
3. Open the backend project folder in Visual Studio 2022.
4. Restore NuGet packages if prompted.
5. Build and run the backend project using Visual Studio (press **F5** or use the run button).
6. Open the frontend folder in a terminal.
7. Run `npm install` to install frontend dependencies.
8. Run `npm start` to launch the React development server.

## Notes

- Make sure your backend server is running before starting the frontend.
- Adjust connection strings and API URLs if needed for your environment.
- The project uses React + Vite for the frontend setup.
