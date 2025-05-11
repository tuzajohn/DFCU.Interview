# DFCU.Interview

# Payment API Documentation

## Overview
The Payment API facilitates transactions between a payer and a payee. It supports operations such as initiating payments and retrieving payment statuses.

## Endpoints

### 1. Initiate Payment
- **Method**: `POST`
- **URL**: `/api/payments`
- **Request Body**:


- **Response**:
  - **200 OK**: Payment successful.
  - **400 Bad Request**: Validation errors or payment failure.
  - **100 Continue**: Payment pending.

### 2. Get Payment Status
- **Method**: `GET`
- **URL**: `/api/payments/{paymentId}/status`
- **Response**:
  - **200 OK**: Payment status retrieved.
  - **400 Bad Request**: Invalid payment ID.
    
### 3. Get Payment Details
- **Method**: `GET`
- **URL**: `/api/payments/{paymentId}`
- **Response**:
  - **200 OK**: Payment details retrieved.
  - **400 Bad Request**: Invalid payment ID.
    
### 4. Get Payments
- **Method**: `GET`
- **URL**: `/api/payments`
- **Response**:
  - **200 OK**: Payments retrieved.

## Models

### PaymentRequest
- `Payer`: Required, 10-digit numeric string.
- `PayerReference`: Optional, string.
- `Payee`: Required, 10-digit numeric string.
- `Currency`: Required, ISO currency code.
- `Amount`: Required, decimal.

### PaymentStatus
- `Pending`: Payment is in progress.
- `Successful`: Payment completed successfully.
- `Failed`: Payment failed.

---

# Deployment Guide

## Client Project (Razor Pages)

### Steps
1. **Build the Project**:
   - In Visual Studio, go to __Build > Build Solution__ or press `Ctrl+Shift+B`.

2. **Publish the Project**:
   - Right-click the Razor Pages project in __Solution Explorer__.
   - Select __Publish__.
   - Choose a target (e.g., Azure, Folder, IIS).
   - Configure the settings and click __Publish__.

3. **Deploy to Server**:
   - If publishing to a folder, copy the output to the web server.
   - Ensure the server has the required .NET runtime installed.

4. **Verify Deployment**:
   - Access the deployed site in a browser to confirm functionality.

## API Project

### Steps
1. **Build the API**:
   - In Visual Studio, go to __Build > Build Solution__ or press `Ctrl+Shift+B`.

2. **Publish the API**:
   - Right-click the API project in __Solution Explorer__.
   - Select __Publish__.
   - Choose a target (e.g., Azure App Service, Folder, Docker).
   - Configure the settings and click __Publish__.

3. **Deploy to Server**:
   - If publishing to a folder, copy the output to the server.
   - Ensure the server has the required .NET runtime installed.

4. **Verify Deployment**:
   - Test the API endpoints using tools like Postman or Swagger.

---

# Running Migrations

## Prerequisites
- Ensure the database connection string is correctly configured in the `appsettings.json` file of the API project.
- Verify that the Entity Framework Core package is installed in the API project.

## Steps

1. **Add a Migration**:
   - Open the Package Manager Console in Visual Studio: __Tools > NuGet Package Manager > Package Manager Console__.
   - Set the default project to the API project.
   - Run the following command:
   - This creates a migration file in the `Migrations` folder.

2. **Apply the Migration**:
   Run the following command in the Package Manager Console:
   - This applies the migration to the database, creating the necessary tables.

3. **Verify the Database**:
   - Check the database to ensure the tables (e.g., `Payments`) are created as per the migration.

## Additional Notes
- If you need to reset the database, you can use:
  - For automated migrations during deployment, ensure the `Program.cs` file includes logic to apply migrations at runtime:
  
---
