# Warehouse Management System (WMS)

A backend-focused Warehouse Management System built with ASP.NET Core and Entity Framework Core.

---

# Project Purpose

The goal of this project is to design and build a business-driven Warehouse Management System (WMS) that focuses on:

- Inventory Integrity
- Transaction Traceability
- Role-Based Security
- Business Rule Enforcement
- Operational Accuracy

The system is built around business events, business rules, and data integrity rather than simple CRUD operations.

---

# Business Philosophy

Business creates events.

Events create data.

Data creates system requirements.

Technology implements system requirements.

A system is successful when business rules survive real-world user behavior, not when code simply returns `200 OK`.

---

# Core Development Principle

```text
Retrieve
↓
Validate
↓
Mutate
↓
Persist
↓
Verify
```

---

# Domain Model

## Master Data

Responsible for defining the structure of operations.

### Warehouse

Primary inventory container.

### Product

Inventory item definition.

### User

System identity.

### Role

Authorization and security.

### UserRole

Role assignment.

---

## Transaction Data

Responsible for recording business activity.

### Inventory

Current inventory state.

### Transfer

Inventory movement event.

### TransferItem

Inventory movement details.

---

# Warehouse Types

## WH-PROCESS

Operational warehouse used for active inventory.

## WH-STORAGE

Reserve warehouse used for storage inventory.

---

# Product Example

SKU

```text
APL16PMM512
```

Product Name

```text
APPLE iPhone 16 Pro Max Midnight 512GB
```

---

# Transfer Workflow

Example:

```text
Transfer 20 iPhones

FROM:
WH-PROCESS

TO:
WH-STORAGE
```

## Workflow

```text
Retrieve
↓
Validate
↓
Mutate
↓
Persist
↓
Verify
```

---

## Validation Rules

- Source Warehouse Exists
- Destination Warehouse Exists
- Source Warehouse != Destination Warehouse
- Product Exists
- Source Inventory Exists
- Quantity > 0
- Sufficient Inventory

---

## Mutation Rules

- Create Transfer
- Create TransferItem
- Reduce Source Inventory
- Increase Destination Inventory

---

# Authentication & Authorization

JWT is used as the Identity Layer.

```text
JWT
↓
Identity Truth

WMS
↓
Transaction Truth
```

---

## Authentication Features

- User Management
- Role Management
- User Role Assignment
- Password Hashing
- Secure Login
- JWT Token Generation
- JWT Token Validation

---

## Authorization Features

- Role-Based Access Control (RBAC)
- Claims-Based Authorization
- Admin Authorization
- Operator Authorization
- InventoryControl Authorization

---

# Business Roles

## Admin

Master Data Authority.

Responsibilities:

- Manage Users
- Manage Roles
- Assign Roles
- Manage Warehouses
- Manage Products

---

## Operator

Transaction Authority.

Responsibilities:

- Process Transfers
- Process Inventory Transactions

---

## InventoryControl

Validation Authority.

Future Responsibilities:

- Cycle Count
- Inventory Variance Investigation
- Inventory Accuracy Validation

---

# Security Testing Completed

## Authentication

Valid Login

Invalid Username

Invalid Password

Inactive User

---

## JWT Testing

Token Generation

Claims Validation

Token Decoding

JWT Signature Validation

---

## Authorization

401 Unauthorized Testing

403 Forbidden Testing

Admin Access Verification

Operator Access Verification

Role-Based Access Control

---

# Current Features

## Warehouse

- Create
- Read
- Update

## Product

- Create
- Read
- Update

## Inventory

- Create
- Read
- Update

## Transfer

- Create
- Read

## TransferItem

- Create
- Read

## Authentication

- User
- Role
- UserRole
- JWT
- Authorization

---

# Learning Outcomes

- ASP.NET Core
- Entity Framework Core
- JWT Authentication
- JWT Authorization
- Role-Based Access Control
- Business Rule Validation
- Data Integrity Validation
- Inventory State Management
- Enterprise Backend Design

---

# Current Status

Warehouse Management

Product Management

Inventory Management

Transfer Management

User Management

Role Management

User Role Assignment

JWT Authentication

JWT Authorization

Role-Based Access Control

---

# Next Roadmap

## Near Term

- Audit Trail
- Cycle Count
- Inventory Variance Tracking

## Long Term

- SignalR Real-Time Updates
- SCADA Integration (TwinCAT)
- Reporting Platform
- Power BI Analytics
- Operational Intelligence Platform
`