<div align="center">

# ⚡ Team Task Manager

### A Full-Stack Collaborative Project & Task Management Platform

[![Live Demo](https://img.shields.io/badge/🚀_Live_Demo-Railway-6366F1?style=for-the-badge)](https://team-task-manager-production-5113.up.railway.app)
[![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular_17-DD0031?style=for-the-badge&logo=angular&logoColor=white)](https://angular.io/)
[![CouchDB](https://img.shields.io/badge/CouchDB-E42528?style=for-the-badge&logo=apache-couchdb&logoColor=white)](https://couchdb.apache.org/)

<br/>

<img src="https://img.shields.io/badge/Status-Live_✅-10B981?style=flat-square" />
<img src="https://img.shields.io/badge/License-MIT-blue?style=flat-square" />
<img src="https://img.shields.io/badge/PRs-Welcome-brightgreen?style=flat-square" />

</div>

---

## 📋 Overview

**Team Task Manager** is a production-ready, full-stack web application that enables teams to collaboratively manage projects, assign tasks, track progress, and maintain accountability — all through a sleek, modern dark-themed interface.

Built with a **.NET 10 Web API** backend, **Angular 17** frontend, and **CouchDB** as the NoSQL database, this application demonstrates industry-standard practices including JWT authentication, role-based access control, RESTful API design, and cloud deployment.

---

## ✨ Key Features

| Feature | Description |
|---------|-------------|
| 🔐 **JWT Authentication** | Secure signup/login with token-based authentication and session management |
| 👥 **Role-Based Access Control** | Admin and Member roles with granular permission control |
| 📁 **Project Management** | Create, view, and manage multiple projects with descriptions |
| ✅ **Task Lifecycle** | Full task workflow: **To Do** → **In Progress** → **Done** with visual status badges |
| 📊 **Real-Time Dashboard** | Live stats with animated progress bars, completion rates, and overdue tracking |
| 👤 **User Management** | Admin panel to approve new users, assign roles, and manage team members |
| ⏰ **Overdue Detection** | Automatic detection and highlighting of overdue tasks |
| 🌙 **Premium Dark UI** | Modern dark theme with glassmorphism, micro-animations, and smooth transitions |
| 📱 **Responsive Design** | Fully responsive across desktop, tablet, and mobile devices |

---

## 🛠️ Tech Stack

### Backend
| Technology | Purpose |
|-----------|---------|
| **.NET 10** | Web API framework |
| **C#** | Backend language |
| **CouchDB** | NoSQL document database |
| **JWT Bearer** | Authentication & authorization |
| **BCrypt** | Password hashing |
| **Swagger/OpenAPI** | API documentation |

### Frontend
| Technology | Purpose |
|-----------|---------|
| **Angular 17** | SPA framework (Standalone Components) |
| **TypeScript** | Type-safe frontend logic |
| **RxJS** | Reactive state management |
| **CSS3** | Custom styling with CSS variables, gradients & animations |
| **Inter Font** | Modern typography via Google Fonts |

### Infrastructure
| Technology | Purpose |
|-----------|---------|
| **Docker** | Containerized deployment |
| **Nginx** | Frontend static serving & API reverse proxy |
| **Railway** | Cloud hosting platform |
| **GitHub** | Source control & CI/CD trigger |

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────┐
│                    RAILWAY CLOUD                     │
│                                                     │
│  ┌──────────────────┐     ┌──────────────────────┐  │
│  │  Team-Task-Mgr   │     │  vigilant-freedom    │  │
│  │  (Frontend)       │────▶│  (Backend API)       │  │
│  │                   │     │                      │  │
│  │  Angular + Nginx  │     │  .NET 10 Web API     │  │
│  │  Port: 80         │     │  Port: 8080          │  │
│  └──────────────────┘     └──────────┬───────────┘  │
│                                      │              │
│                           ┌──────────▼───────────┐  │
│                           │  CouchDB             │  │
│                           │  (NoSQL Database)    │  │
│                           │  Port: 5984          │  │
│                           └──────────────────────┘  │
└─────────────────────────────────────────────────────┘
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) & npm
- [CouchDB](https://couchdb.apache.org/) (local or cloud instance)
- [Git](https://git-scm.com/)

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/akhilkaushik024/Team-Task-Manager.git
cd Team-Task-Manager
```

### 2️⃣ Backend Setup

```bash
cd Backend

# Update CouchDB connection in appsettings.json
# Set your CouchDB URL, username, password, and database name

dotnet restore
dotnet run
```

The API will start at `http://localhost:5120`

### 3️⃣ Frontend Setup

```bash
cd Frontend

npm install
ng serve
```

The app will open at `http://localhost:4200`

### 4️⃣ Environment Variables (Production)

| Variable | Value | Description |
|----------|-------|-------------|
| `COUCHDB_URL` | `https://your-couchdb-url` | CouchDB connection URL |
| `COUCHDB_USERNAME` | `your-username` | CouchDB admin username |
| `COUCHDB_PASSWORD` | `your-password` | CouchDB admin password |
| `COUCHDB_DATABASE` | `teamtaskmanager` | Database name |
| `ASPNETCORE_URLS` | `http://+:8080` | Server binding |

---

## 📡 API Endpoints

### Authentication
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/auth/register` | Register a new user | ❌ |
| `POST` | `/api/auth/login` | Login and receive JWT | ❌ |

### Projects
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/projects` | List all projects | ✅ |
| `POST` | `/api/projects` | Create a new project | ✅ Admin |
| `DELETE` | `/api/projects/{id}` | Delete a project | ✅ Admin |

### Tasks
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/tasks` | List all tasks | ✅ |
| `POST` | `/api/tasks` | Create a new task | ✅ Admin |
| `PUT` | `/api/tasks/{id}/status` | Update task status | ✅ |
| `DELETE` | `/api/tasks/{id}` | Delete a task | ✅ Admin |

### Users
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/users` | List all users | ✅ Admin |
| `PUT` | `/api/users/{id}/approve` | Approve a user | ✅ Admin |
| `PUT` | `/api/users/{id}/role` | Update user role | ✅ Admin |

---

## 📁 Project Structure

```
Team-Task-Manager/
├── Backend/                    # .NET 10 Web API
│   ├── Controllers/
│   │   ├── AuthController.cs       # Authentication endpoints
│   │   ├── ProjectsController.cs   # Project CRUD
│   │   ├── TasksController.cs      # Task CRUD & status updates
│   │   └── UsersController.cs      # User management (Admin)
│   ├── Models/                     # Data models
│   ├── Services/
│   │   ├── CouchDbService.cs       # Database operations
│   │   └── JwtService.cs           # JWT token generation
│   ├── Program.cs                  # App configuration & middleware
│   ├── Dockerfile                  # Backend container config
│   └── Backend.csproj
│
├── Frontend/                   # Angular 17 SPA
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   │   ├── dashboard/      # Stats & progress overview
│   │   │   │   ├── login/          # Authentication
│   │   │   │   ├── register/       # User registration
│   │   │   │   ├── navbar/         # Navigation bar
│   │   │   │   ├── projects/       # Project management
│   │   │   │   ├── tasks/          # Task management
│   │   │   │   └── users/          # User admin panel
│   │   │   ├── services/           # API & auth services
│   │   │   ├── guards/             # Route guards
│   │   │   ├── interceptors/       # HTTP interceptors
│   │   │   └── models/             # TypeScript interfaces
│   │   ├── styles.css              # Global design system
│   │   └── environments/
│   ├── Dockerfile                  # Frontend container config
│   ├── nginx.conf                  # Nginx reverse proxy config
│   └── angular.json
│
├── Dockerfile                  # Root unified build (optional)
├── railway.toml                # Railway deployment config
└── README.md
```

---

## 🎨 UI Highlights

- **Dark Premium Theme** — Refined dark palette (`#0F1729`) with subtle purple/cyan ambient gradients
- **Glassmorphism Cards** — Frosted glass effect with backdrop blur and translucent borders
- **Micro-Animations** — Staggered card entrances, badge pulses, shimmer effects, input glows
- **Gradient Accents** — Purple-to-violet buttons, progress bars, and accent borders
- **Responsive Typography** — Inter font with carefully weighted heading hierarchy

---

## 🔒 Security

- **Password Hashing** — BCrypt with salt rounds for secure password storage
- **JWT Tokens** — Short-lived access tokens for API authentication
- **Role Guards** — Frontend route guards prevent unauthorized access
- **HTTP Interceptors** — Automatic token injection on API requests
- **User Approval Flow** — New users require admin approval before accessing the system
- **CORS Protection** — Configured allowed origins for production

---

## 🌐 Live Deployment

| Service | URL |
|---------|-----|
| 🖥️ **Frontend** | [team-task-manager-production-5113.up.railway.app](https://team-task-manager-production-5113.up.railway.app) |
| ⚙️ **Backend API** | [vigilant-freedom-production.up.railway.app](https://vigilant-freedom-production.up.railway.app) |
| 🗄️ **CouchDB** | Private (Railway internal) |

---

## 👨‍💻 Author

**Akhil Kaushik**

[![GitHub](https://img.shields.io/badge/GitHub-akhilkaushik024-181717?style=flat-square&logo=github)](https://github.com/akhilkaushik024)

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

<div align="center">

**Built with ❤️ using .NET, Angular & CouchDB**

⭐ Star this repo if you found it useful!

</div>
