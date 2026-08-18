import { useState, useEffect } from 'react'
import { Link, useLocation } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { InlineError } from '../components/ui'

export default function LoginPage() {
  const { login, loading, error, clearError, isAuthenticated } = useAuth()
  const location = useLocation()

  const [form, setForm] = useState({ email: '', password: '' })
  const [localError, setLocalError] = useState('')

  useEffect(() => { clearError() }, [form])

  const validate = () => {
    if (!form.email.trim()) return 'Email is required.'
    if (!/\S+@\S+\.\S+/.test(form.email)) return 'Enter a valid email address.'
    if (!form.password) return 'Password is required.'
    return null
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLocalError('')
    const err = validate()
    if (err) { setLocalError(err); return }
    await login(form.email.trim(), form.password)
  }

  const from = location.state?.from?.pathname || '/dashboard'

  return (
    <div className="min-h-screen flex items-center justify-center px-4">
      <div className="w-full max-w-sm animate-slide-up">
        
        <div className="text-center mb-8">
          <div className="inline-flex items-center justify-center w-12 h-12 rounded-xl bg-cyan-glow/10 border border-cyan-glow/20 mb-4">
            <svg className="w-6 h-6 text-cyan-glow" fill="currentColor" viewBox="0 0 24 24">
              <path d="M17 3H7a2 2 0 00-2 2v14a2 2 0 002 2h10a2 2 0 002-2V5a2 2 0 00-2-2zm-5 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/>
            </svg>
          </div>
          <h1 className="font-display font-bold text-2xl text-white tracking-wide">Welcome back</h1>
          <p className="text-sm text-gray-500 mt-1 font-body">Sign in to your GameData account</p>
        </div>

        <div className="card p-6 space-y-4">
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="label">Email</label>
              <input
                type="email"
                className="input-field"
                placeholder="you@example.com"
                value={form.email}
                onChange={(e) => setForm((p) => ({ ...p, email: e.target.value }))}
                autoComplete="email"
                autoFocus
              />
            </div>

            <div>
              <label className="label">Password</label>
              <input
                type="password"
                className="input-field"
                placeholder="••••••••"
                value={form.password}
                onChange={(e) => setForm((p) => ({ ...p, password: e.target.value }))}
                autoComplete="current-password"
              />
            </div>

            {(localError || error) && <InlineError message={localError || error} />}

            <button
              type="submit"
              disabled={loading}
              className="btn-primary w-full flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <div className="w-4 h-4 border-2 border-bg-primary/30 border-t-bg-primary rounded-full animate-spin" />
                  Signing in…
                </>
              ) : (
                'Sign in'
              )}
            </button>
          </form>
        </div>

        <p className="text-center text-sm text-gray-500 mt-5 font-body">
          Don't have an account?{' '}
          <Link to="/register" className="text-cyan-glow hover:text-cyan-soft transition-colors font-medium">
            Create one
          </Link>
        </p>
      </div>
    </div>
  )
}
