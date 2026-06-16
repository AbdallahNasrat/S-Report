# 🚨 S-Report (Smart Reporting System)

> An AI-powered, real-time emergency and incident reporting system designed to bridge the gap between citizens, volunteers, and government authorities.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Flutter](https://img.shields.io/badge/Flutter-02569B?style=for-the-badge&logo=flutter&logoColor=white)
![AI/ML](https://img.shields.io/badge/AI_Models-Hugging_Face-FFD21E?style=for-the-badge&logo=huggingface&logoColor=black)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![SignalR](https://img.shields.io/badge/Real_Time-SignalR-0078D4?style=for-the-badge&logo=microsoft&logoColor=white)

## 📖 About The Project
**S-Report** is a graduation project aimed at streamlining how incidents are reported and resolved. Citizens can report issues via a mobile app, which are then analyzed automatically by Artificial Intelligence to determine the emergency type and priority. The system dispatches these reports in real-time to the appropriate department dashboard and alerts nearby volunteers using geospatial algorithms.

## 📂 Repository Structure (Monorepo)
This repository contains the complete source code for the entire system, separated into three main micro-components:

```text
📦 S-Report-System
 ┣ 📂 Backend-API        # Core .NET 8 API (Clean Architecture, SignalR, EF Core)
 ┣ 📂 AI-Service         # Python/Hugging Face service for Image/Audio/Text analysis
 ┣ 📂 Frontend-Flutter   # Mobile Application for Citizens & Volunteers (Cross-platform)
 ┣ 📂 Frontend         # Web Site For Employees & Admin
 ┣ 📂 BI-Service         # Dashboard & Live Map
 ┗ 📜 README.md


✨ Key Features
🤖 Smart AI Analysis: Automatic extraction of report category and priority level from user-submitted images, text, and voice records without blocking the UI.

⚡ Real-Time Dashboard: Employee web interface updated instantly using SignalR WebSockets, ensuring zero-delay notifications during emergencies.

📍 Geospatial Filtering: Employs the Haversine Formula to precisely calculate distances and push missions to volunteers within a 10 KM radius.

🔐 Secure Access: Role-Based Access Control (RBAC) and JWT Authentication separating Citizens, Volunteers, and Admins securely.

🏛️ Backend Architecture Highlights
The backend .NET 8 API is engineered for high performance and scalability:

Clean Architecture: Strict separation of concerns (Domain, Application, Infrastructure, Presentation).

Design Patterns: Implemented Generic Repositories and Unit of Work for safe database transactions.

Asynchronous Processing: Heavy AI tasks are offloaded to background threads to maintain a fast, non-blocking user experience.

Optimized Payloads: Custom Data Transfer Objects (DTOs) and safe Null-handling to guarantee zero-downtime and minimal network usage.

👨‍💻 Development Team
Developed as a graduation project by a dedicated team of software engineers:

[Abdallah Nasrat - Backend Engineer (.NET / Architecture)

[Mohamed Jad - Abdelrahman Akram - Zeyad Ahmed ] - Frontend  & Flutter Developers 

[Mahmoud Ahmed  - Ahmed Ayman  - Alaa Abdelkareem] - AI Developers 

[Youssef Ibrahim] - Data Analysis 

Built with passion to innovate public safety and incident management.
