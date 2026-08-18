# Game Data Frontend Dashboard

A production-ready React dashboard for the GameData .NET backend.

## Tech Stack

- **React 18** + **Vite** — fast dev server & build
- **Tailwind CSS** — utility-first dark gaming theme
- **Axios** — HTTP client with JWT interceptors
- **React Router DOM v6** — client-side routing with protected routes

## Project Structure

```
src/
 ├── components/
 │   ├── AppLayout.jsx       # Sidebar + main content shell
 │   ├── ProtectedRoute.jsx  # Auth guard
 │   ├── Sidebar.jsx         # Navigation
 │   └── ui.jsx              # StatCard, LoadingSpinner, ErrorState, etc.
 ├── context/
 │   ├── AuthContext.jsx     # Global auth state + login/register/logout
 │   └── ToastContext.jsx    # Toast notification system
 ├── hooks/
 │   └── useAsync.js         # Generic async state wrapper
 ├── pages/
 │   ├── LoginPage.jsx
 │   ├── RegisterPage.jsx
 │   ├── DashboardPage.jsx
 │   ├── LeaderboardPage.jsx
 │   ├── AchievementsPage.jsx
 │   └── SessionsPage.jsx
 ├── services/
 │   ├── api.js              # Axios instance + interceptors
 │   ├── authService.js      # Auth API calls
 │   ├── gameService.js      # Game progress API calls
 │   └── index.js            # Leaderboard, achievement, session services
 ├── utils/
 │   └── helpers.js          # formatNumber, formatDuration, timeAgo, etc.
 ├── App.jsx                 # Routes + provider composition
 ├── main.jsx                # Entry point
 └── index.css               # Tailwind base + custom component classes
```

## Getting Started

```bash
# 1. Install dependencies
npm install

# 2. Configure environment
cp .env.example .env
# Edit VITE_API_BASE_URL if your backend runs on a different port

# 3. Start dev server
npm run dev
# → http://localhost:3000

# 4. Build for production
npm run build
```

## Routes

| Path            | Access     | Description             |
|-----------------|------------|-------------------------|
| `/login`        | Public     | Sign in                 |
| `/register`     | Public     | Create account          |
| `/dashboard`    | Protected  | Stats + game progress   |
| `/leaderboard`  | Protected  | Top 10 players          |
| `/achievements` | Protected  | Unlocked vs locked      |
| `/sessions`     | Protected  | Session history         |

## Backend API

Expects the GameDataBackend .NET API running at `http://localhost:5000`.

All authenticated requests automatically receive the `Authorization: Bearer <token>` header via the Axios request interceptor. A 401 response automatically clears the token and redirects to `/login`.

## Key Design Decisions

- **AuthContext** persists `token` + `user` in `localStorage` — sessions survive page refresh.
- **useAsync** hook eliminates boilerplate `loading/error/data` state across all pages.
- **ToastProvider** renders a fixed overlay container; pages call `toast.success/error()` without caring about rendering.
- **ProtectedRoute** saves the original URL in router state so the user lands back after login.
