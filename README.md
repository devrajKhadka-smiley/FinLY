# 📊 FinLY (Personal Finance Manager)

This project is a prototype desktop application designed for personal expense tracking, developed in C# using the .NET Core framework. The application enables users to manage their cash inflows, outflows, and debts effectively while providing insightful dashboards and customizable tagging options.

---

## ✨ Features

### 🔑 User Setup
- **First-Time Setup**: On the first startup, the application prompts users to provide:
  - Username
  - Application password
  - Select Currency Type previously set while creating the account.

### 💰 Core Functionalities
1. **Cash Inflows (Credit, Gain, or Budget)**
   - Track all income sources and gains.
2. **Cash Outflows (Debit, Spending, or Expenses)**
   - Ensure transactions occur only if sufficient balance is available.
3. **Debt Tracking**
   - Fields include the source of debt, due date, and notes.
   - Highlight pending debts and ensure they are cleared from cash inflows only.

### 🔄 Transaction Features
- Categorize transactions as **credit**, **debit**, or **debt**.
- Add optional notes for additional details.
- Tag transactions with **custom labels** such as "monthly," "rent," or "groceries."
- Default Tags include: `Yearly`, `Monthly`, `Food`, `Drinks`, `Clothes`, `Gadgets`, `Miscellaneous`, `Fuel`, `Rent`, `EMI`, `Party`, etc.

### 🔍 Search and Filter
- Search, filter, and sort records by:
  - Title
  - Transaction type
  - Tags
  - Date range
  - Specific dates

### 📊 Dashboard
- Provides detailed statistics such as:
  - Total cash inflows, outflows, debts, and cleared debts
  - Remaining debts
  - Records of the **top 5 highest or lowest transactions**
- List of pending debts, with filtering options by date range.

### 🗎️ Data Import/Export
- Import and export data in structured formats:
  - JSON
---

## 🛠️ Getting Started

### 🔁 Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/devrajKhadka-smiley/FinLY.git
2. navigate to the project - directory
    ```bash
    cd FinLY

## 📖 Usage

1. Launch the application.
2. Provide the required setup details (username, password, and currency type).
3. Use the main interface to:
   - Add and manage transactions.
   - Track debts and clear them using cash inflows.
   - Use the dashboard for a summarized view of your finances.
   - Export data for external usage or import pre-existing data.

---

## 💕 Acknowledgments

- Developed Application for coursework project.
