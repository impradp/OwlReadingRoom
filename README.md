# 🦉 Owl Reading Room

## 📋 Overview

Owl Reading Room is a Windows-based desktop application designed for enterprise-level management of reading space bookings. This versatile solution enables efficient resource allocation, package creation, and customer management for study space providers. While currently optimized for Windows, the application's codebase is designed for cross-platform compatibility, allowing potential future deployment on iOS and Android devices.

## ✨ Features

- 🏢 Resource Management
  - 🚪 Room: Manage and organize reading spaces efficiently
  - 📦 Package: Create and customize booking packages for different user needs
- 👥 Customer Management: Maintain customer profiles and preferences
- 📅 Booking Management: Streamline the process of reserving reading spaces

## 🛠️ Technologies Used

- .NET MAUI
- C#
- XAML
- SQLite (As SQL Database Engine)
- LINQ (As QueryBuilder)

## 🚀 Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 (17.3 or later) with the .NET MAUI workload installed
- For macOS: Visual Studio 2022 for Mac (17.4 or later)

### 📚 Documentation
- 📝 Project Design and Discovery: [Google Doc](https://docs.google.com/document/d/1huzc70ilRKnFnaGhGPMki97ia5-pndBhzQ_eBbaWY0c/edit)
- 📊 Scrum Board: [Jira](https://blackpebble.atlassian.net/jira/software/projects/ORR/list)
- 🎨 Design: [Figma](https://www.figma.com/files/project/238611168)
- 🏗️ Architecture Diagram: [Draw.io](https://app.diagrams.net/#G15ZMyF-kgphy_5FYp1H_TYMiDlTf1Mb0Q)
- 📊 Class Diagrams: [Draw.io](https://app.diagrams.net/#G15ZMyF-kgphy_5FYp1H_TYMiDlTf1Mb0Q#%7B%22pageId%22%3A%22HRrueGPj96ZWi6Z7gjnQ%22%7D)
- 🔄 Flow Diagrams: [Draw.io](https://app.diagrams.net/#G15ZMyF-kgphy_5FYp1H_TYMiDlTf1Mb0Q#%7B%22pageId%22%3A%22_hAMUAXjWXnGv0WOe4Ry%22%7D)
- 🗄️ ER Diagrams: [DBdiagram.io](https://dbdiagram.io/d/Owl-Reading-Room-DB-v2-66ccb8b43f611e76e987adb2)

### 📦 Publish Guidelines
Owl Reading Room is published with the generic windows keys created using the deployment scripts. The script is run from package manager console and the output folder will be updated with the latest msxi file for the project.

### 🚀 Release Plans
Projects released are based on the scope and are tagged with semantic tags eg: v1.0.0 or v2.3.4. Hop into the [project release section](https://github.com/impradp/OwlReadingRoom/releases) to view the latest releases.

### 💻 Installation

1. Clone the repository:
   ```
   git clone git@github.com:impradp/OwlReadingRoom.git
   ```
2. Open the solution in Visual Studio 2022.
3. Restore NuGet packages.
4. Build the solution.

## 🔧 Usage

In order to run this application locally, we need to get updated access codes from the contributors. Please ask them for those credentials.

## 📁 Project Structure

- `App.xaml`: Contains all the resources of the application including styles, colors and custom utilities like converters.
- `Models/`: Data models used in the application to fetch and store application specific data into the application.
- `ViewModels/`: ViewModels for implementing MVVM pattern and align the binding and property change functionality with the desired global variable.
- `Views/`: Additional XAML views to display different layouts for room creation/management, package creation/management, customer creation/management and booking management.
- `Services/`: Business logic and data access services to handle all the data level functionality across application.
- `Components/`: Contains generic components like alert dialog, action buttons and loaders used by the application.
- `Configurations/`: Maintains and extracts the configurations related to the appsettings variables maintained for the application. 
- `DTOs/`: Contains the data transfer objects created for data manipulation and transfer. 
- `Events/`: Contains list of events that manages the trigger functionalities to properly subscribe and publish the triggers to needed components or layouts. 
- `Platforms/`: Maintains the platform specific configurations with .Net MAUI single code multi platform approach.
- `Proxy/`: Contains custom transactional proxies created to properly handle the transaction scope for the application.

## 🚧 Current Status

In active development.

## 🆘 Support

For support, please contact the contributors listed below. If you encounter any issues, please report them through email.

## 👥 Contributors

- Pradip Puri - [@impradp](https://github.com/impradp) - impradp@gmail.com
- Mohendra Amatya - [@JVMA1994](https://github.com/JVMA1994) - amatya.mohendra@gmail.com
- Dilisha Maharjan (Designer) - dilishamaharjan11@gmail.com

## 📄 License

All rights reserved. This project is proprietary and confidential. Unauthorized copying, modification, distribution, or use of this software, via any medium, is strictly prohibited.