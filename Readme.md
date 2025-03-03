# ASP.NetCore_RetailStore

## Overview

ASP.NetCore_RetailStore is a web application designed as a **Point of Sale (POS)** system for a retail store specializing in **mobile phones and accessories**. The system provides essential functionalities such as sales transactions, product management, employee management, customer tracking, and reporting.

This project was developed as part of the **Web Programming with Node.js** final project for **Semester 1/2023-2024**.

## Technologies Used
### **Backend**
- **ASP.NET Core with Razor Pages**: Provides the server-side logic and dynamic UI rendering.
- **MongoDB**: NoSQL database for storing products, customers, and transaction data.
- **ASP.NET Identity & JWT Authentication**: Secure login and user authentication.
- **SMTP (MailKit)**: Sends automated emails for account notifications.

### **Frontend**
- **Razor Pages**: Implements a simple yet powerful UI framework within ASP.NET Core.
- **Bootstrap / Tailwind CSS**: Enhances UI design and responsiveness.

### **Additional Tools & Services**
- **Entity Framework Core (MongoDB Provider)**: Manages database operations.
- **AutoMapper**: Simplifies object mapping between models and DTOs.
- **Serilog**: Implements structured logging.
- **Docker**: Containerization for deployment.


## Features

### **Account Management**
- Pre-configured admin account (`admin/admin`).
- Sales staff accounts are created by the admin.
- Automatic email notification for new accounts.
- Temporary login links expire in **1 minute**.
- First-time login requires a password reset.
- Login using username (email prefix).
- Profile management (avatar, name, password change).

### **User Management (Admin Only)**
- View the staff list.
- View detailed employee profiles.
- Resend login email.
- Lock/unlock employee accounts.

### **Product Management**
- Admin can **view, add, update, and delete** products.
- Employees can only view product lists (without wholesale price).
- Products cannot be deleted if linked to a purchase.

### **Customer Management**
- Customer accounts are created automatically on first purchase.
- View customer details (name, phone, address).
- Track purchase history and order details.

### **Transaction Processing**
- Add products via **search** or **barcode scanning**.
- View and edit cart in real-time.
- Automatic order summary updates (total price, change calculation).
- Process transactions and generate invoices (PDF).

### **Reporting and Analytics**
- View sales reports by predefined periods (today, last 7 days, this month, custom date range).
- Display key metrics: total sales, number of orders, products sold.
- Admin can see **profit calculations**.

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/KTPx4/ASP.NetCore_RetailStore.git
   cd ASP.NetCore_RetailStore
   ```

2. Set up the database connection in `appsettings.json`.

3. Install dependencies:
   ```bash
   dotnet restore
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to:
   ```
   http://localhost:5000
   ```

