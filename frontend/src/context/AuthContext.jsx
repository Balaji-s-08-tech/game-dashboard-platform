import { createContext, useContext, useState, useCallback, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { authService } from '../services/authService'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const navigate = useNavigate()

  const [user, setUser] = useState(() => {
    try {
      const stored = localStorage.getItem('user')
      return stored ? JSON.parse(stored) : null
    } catch {
      return null
    }
  })

  const [token, setToken] = useState(() => localStorage.getItem('token') || null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  const isAuthenticated = !!token && !!user

  useEffect(() => {
    if (token && user) {
      localStorage.setItem('token', token)
      localStorage.setItem('user', JSON.stringify(user))
    } else {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    }
  }, [token, user])

  const login = useCallback(async (email, password) => {
    setLoading(true)
    setError(null)
    try {
      const data = await authService.login(email, password)
      setToken(data.token)
      setUser({
        id: data.userId,
        username: data.username,
        email: data.email,
        expiresAt: data.expiresAt,
      })
      navigate('/dashboard', { replace: true })
      return { success: true }
    } catch (err) {
      const message = err.response?.data?.message || 'Login failed. Please try again.'
      setError(message)
      return { success: false, message }
    } finally {
      setLoading(false)
    }
  }, [navigate])

  const register = useCallback(async (username, email, password) => {
    setLoading(true)
    setError(null)
    try {
      const data = await authService.register(username, email, password)
      setToken(data.token)
      setUser({
        id: data.userId,
        username: data.username,
        email: data.email,
        expiresAt: data.expiresAt,
      })
      navigate('/dashboard', { replace: true })
      return { success: true }
    } catch (err) {
      const message = err.response?.data?.message || 'Registration failed. Please try again.'
      setError(message)
      return { success: false, message }
    } finally {
      setLoading(false)
    }
  }, [navigate])

  const logout = useCallback(() => {
    setToken(null)
    setUser(null)
    setError(null)
    navigate('/login', { replace: true })
  }, [navigate])

  const clearError = useCallback(() => setError(null), [])

  const refreshProfile = useCallback(async () => {
    if (!token) return
    try {
      const profile = await authService.getProfile()
      setUser((prev) => ({ ...prev, ...profile }))
    } catch {

    }
  }, [token])

  const value = {
    user,
    token,
    loading,
    error,
    isAuthenticated,
    login,
    register,
    logout,
    clearError,
    refreshProfile,
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within an AuthProvider')
  return ctx
}
