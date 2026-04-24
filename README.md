# ⚔️ MinecraftMini - Weapon Inventory Management System

[![Language: C#](https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Status](https://img.shields.io/badge/Status-Completed-brightgreen?style=flat-square)]()

A **console-based weapon inventory management system** inspired by Minecraft-style games. Built with **C# / .NET 8**, this project demonstrates fundamental **Object-Oriented Programming (OOP)** principles, **file I/O** with CSV persistence, **full CRUD operations**, and a **formatted console user interface**.

## ✨ Features

- **🧱 OOP Principles in Action**
  - **Encapsulation**: Weapon properties are protected with private fields and public properties.
  - **Validation**: Business rules prevent invalid data (e.g., negative damage values, empty names).
  - **Computed Properties**: Derived attributes like `DisplayInfo` calculated on the fly.

- **💾 Persistent Storage**
  - Weapons are saved to and loaded from `weapons.csv` automatically.
  - Seamless data persistence between application sessions.

- **🔄 Full CRUD Support**
  - **Create** new weapons with custom stats (name, damage, durability).
  - **Read** and display all weapons in a formatted table.
  - **Update** existing weapon attributes.
  - **Delete** unwanted weapons from the inventory.

- **🛡️ Robust Input Validation**
  - Prevents crashes from invalid user entries (letters instead of numbers, empty strings, etc.).
  - Clear error messages guide the user to correct input.

- **🖥️ Polished Console UI**
  - Clean, colorful, and formatted console interface.
  - Interactive menu system for easy navigation.

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later
- Any C# compatible IDE (Visual Studio, VS Code, Rider) or a terminal

### Installation & Running

1. **Clone the repository**
   ```bash
   git clone https://github.com/aaranas3/MinecraftMini.git
   cd MinecraftMini
