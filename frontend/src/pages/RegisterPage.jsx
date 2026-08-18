import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { InlineError } from '../components/ui'

export default function RegisterPage() {
  const { register, loading, error, clearError } = useAuth()

  const [form, setForm] = useState({ username: '', email: '', password: '', confirm: '' })
  const [localError, setLocalError] = useState('')

  useEffect(() => {
    clearError()
    setLocalError('')
  }, [form])

  const validate = () => {
    if (!form.username.trim()) return 'Username is required.'
    if (form.username.trim().length < 3) return 'Username must be at least 3 characters.'
    if (!form.email.trim()) return 'Email is required.'
    if (!/\S+@\S+\.\S+/.test(form.email)) return 'Enter a valid email address.'
    if (!form.password) return 'Password is required.'
    if (form.password.length < 8) return 'Password must be at least 8 characters.'
    if (form.password !== form.confirm) return 'Passwords do not match.'
    return null
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLocalError('')
    const err = validate()
    if (err) { setLocalError(err); return }
    await register(form.username.trim(), form.email.trim(), form.password)
  }

  const field = (key, label, type = 'text', placeholder = '', autoComplete = '') => (
    <div>
      <label className="label">{label}</label>
      <input
        type={type}
        className="input-field"
        placeholder={placeholder}
        value={form[key]}
        onChange={(e) => setForm((p) => ({ ...p, [key]: e.target.value }))}
        autoComplete={autoComplete}
      />
    </div>
  )

  return (
    <div className="min-h-screen flex items-center justify-center px-4">
      <div className="w-full max-w-sm animate-slide-up">
        
        <div className="text-center mb-8">
          <div className="inline-flex items-center justify-center w-12 h-12 rounded-xl bg-cyan-glow/10 border border-cyan-glow/20 mb-4">
            <svg className="w-6 h-6 text-cyan-glow" fill="currentColor" viewBox="0 0 24 24">
              <path d="M17 3H7a2 2 0 00-2 2v14a2 2 0 002 2h10a2 2 0 002-2V5a2 2 0 00-2-2zm-5 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/>
            </svg>
          </div>
          <h1 className="font-display font-bold text-2xl text-white tracking-wide">Create account</h1>
          <p className="text-sm text-gray-500 mt-1 font-body">Join the GameData platform</p>
        </div>

        <div className="card p-6 space-y-4">
          <form onSubmit={handleSubmit} className="space-y-4">
            {field('username', 'Username', 'text', 'your_tag', 'username')}
            {field('email', 'Email', 'email', 'you@example.com', 'email')}
            {field('password', 'Password', 'password', '••••••••', 'new-password')}
            {field('confirm', 'Confirm Password', 'password', '••••••••', 'new-password')}

            {form.password && (
              <PasswordStrength password={form.password} />
            )}

            {(localError || error) && <InlineError message={localError || error} />}

            <button
              type="submit"
              disabled={loading}
              className="btn-primary w-full flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <div className="w-4 h-4 border-2 border-bg-primary/30 border-t-bg-primary rounded-full animate-spin" />
                  Creating account…
                </>
              ) : (
                'Create account'
              )}
            </button>
          </form>
        </div>

        <p className="text-center text-sm text-gray-500 mt-5 font-body">
          Already have an account?{' '}
          <Link to="/login" className="text-cyan-glow hover:text-cyan-soft transition-colors font-medium">
            Sign in
          </Link>
        </p>
      </div>
    </div>
  )
}

function PasswordStrength({ password }) {
  const checks = [
    { label: '8+ characters', pass: password.length >= 8 },
    { label: 'Uppercase', pass: /[A-Z]/.test(password) },
    { label: 'Number', pass: /\d/.test(password) },
    { label: 'Symbol', pass: /[^A-Za-z0-9]/.test(password) },
  ]
  const passed = checks.filter((c) => c.pass).length
  const strength = ['', 'Weak', 'Fair', 'Good', 'Strong'][passed]
  const colors = ['', 'text-red-glow', 'text-amber-glow', 'text-cyan-glow', 'text-green-glow']

  return (
    <div className="space-y-2">
      <div className="flex gap-1">
        {checks.map((c, i) => (
          <div
            key={i}
            className={`h-1 flex-1 rounded-full transition-all duration-300 ${
              c.pass ? 'bg-cyan-glow' : 'bg-bg-border'
            }`}
          />
        ))}
      </div>
      <div className="flex items-center justify-between">
        <div className="flex gap-3">
          {checks.map((c) => (
            <span
              key={c.label}
              className={`text-[10px] font-body transition-colors ${
                c.pass ? 'text-gray-400' : 'text-gray-600'
              }`}
            >
              {c.label}
            </span>
          ))}
        </div>
        <span className={`text-xs font-medium font-body ${colors[passed]}`}>{strength}</span>
      </div>
    </div>
  )
}
