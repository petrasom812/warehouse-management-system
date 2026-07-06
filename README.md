# Warehouse Management System (WMS)

A backend-focused Warehouse Management System built with ASP.NET Core and Entity Framework Core.

## Project Goals

Learn and apply:

- Master Data
- Transaction Data
- Retrieve
- Validate
- Mutate
- Persist

## Domain Model

### Master Data

- Warehouse
- Product
- Inventory

### Transaction Data

- Transfer
- TransferItem

---

## Warehouse Types

### WH-PROCESS

Processing warehouse used for operational inventory.

### WH-STORAGE

Storage warehouse used for reserve inventory.

---

## Product Example

SKU:

APL16PMM512

Product Name:

APPLE iPhone 16 Pro Max Midnight 512GB

---

## Inventory Example

Warehouse:

WH-PROCESS

Product:

APL16PMM512

Quantity:

100

---

## Transfer Workflow

Example:

Transfer 20 iPhones

FROM:

WH-PROCESS

TO:

WH-STORAGE

### Flow

Retrieve

↓

Validate

↓

Mutate

↓

Persist

### Validation

- Source Warehouse Exists
- Destination Warehouse Exists
- Source Warehouse != Destination Warehouse
- Product Exists
- Source Inventory Exists
- Quantity > 0
- Sufficient Inventory

### Mutation

- Create Transfer
- Create TransferItem
- Reduce Source Inventory
- Increase Destination Inventory

---

## Completed Features

### Warehouse

- Create
- Read
- Update
- Delete

### Product

- Create
- Read
- Update
- Delete

### Inventory

- Create
- Read
- Update
- Delete

### Transfer

- Create
- Read

### Transfer Item

- Create
- Read

---

## Learning Outcomes

- EF Core Relationships
- Navigation Properties
- DTO Projections
- Inventory State Management
- Business Rule Validation
- End-to-End Workflow Design

---

## Current Status

Warehouse

Product

Inventory

Transfer

TransferItem

Inventory State Mutation

---

## Next Project

Cycle Count