# 🚚 Vehicle Tracking Desktop App (TCP Listener)

A **desktop application** built to **receive and process real-time vehicle tracking data** using a **TCP Listener**.  
This system is designed for fleet management, IoT tracking devices, and GPS data monitoring in real-time environments.

---

## 🚀 Features

- 📡 **TCP Listener for GPS Data**  
  - Listens on a configurable TCP port for incoming tracking data from GPS devices.  
  - Supports multiple simultaneous device connections.  

- 🛰️ **Real-Time Data Processing**  
  - Parses GPS packets from devices.  
  - Extracts vehicle location, speed, direction, and status.  
  - Displays live data in the application interface.  

- 🗺️ **Data Visualization**  
  - Optionally integrates map display (e.g., Google Maps / Leaflet / Bing Maps).  
  - Tracks active vehicles in real-time.  

- 💾 **Data Storage**  
  - Stores received tracking data into a local or remote database.  
  - Supports SQLite, SQL Server, or MySQL.  

- 🧾 **Logging & Monitoring**  
  - Keeps logs of all received data packets.  
  - Shows active device connection status and history.  

- ⚙️ **Configurable Settings**  
  - Customizable TCP port, database connection, and update interval.  
  - Supports application startup as a Windows service (optional).  

---

## 🛠️ Technologies Used

| Component | Technology |
|-----------|------------|
| **Language** | C# |
| **Framework** | .NET Framework / .NET Core (Desktop) |
| **Networking** | TCP Listener (System.Net.Sockets) |
| **Database** | SQLite / SQL Server |
| **UI Framework** | Windows Forms / WPF |
| **Logging** | NLog / Serilog |

---

## 💻 Installation & Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/VehicleTrackingTcpApp.git

-------------------

🧰 Usage

Launch the application.

Click Start Listener to begin receiving data.

Observe incoming packets and parsed tracking data in the dashboard.

(Optional) View live positions on the integrated map.

Check logs for connection activity and data history.

-----------------------------

📡 Example Data Packet (Device to Server)

Example of a GPS tracking data format (customizable by device type):

IMEI:359710055555555, Lat:23.780573, Lng:90.279239, Speed:45, Time:2025-11-03 12:45:30


Parsed fields include:

Device ID / IMEI

Latitude / Longitude

Speed (km/h)

Timestamp

--------------------------

📄 Example Workflow

GPS Device sends data via TCP →

Application’s TCP Listener receives packet →

Parses and validates the data →

Stores in the database →

Displays live data on dashboard / map

----------------------

📚 Future Enhancements

WebSocket integration for real-time web clients

REST API for data sharing

Map-based UI with playback / history tracking

Geofencing and alerts

Support for multiple GPS protocols
