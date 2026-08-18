export function StatCard({ label, value, icon, accent = 'cyan', sub }) {
  const accentMap = {
    cyan: 'text-cyan-glow bg-cyan-glow/10 border-cyan-glow/20',
    amber: 'text-amber-glow bg-amber-glow/10 border-amber-glow/20',
    green: 'text-green-glow bg-green-glow/10 border-green-glow/20',
    red: 'text-red-glow bg-red-glow/10 border-red-glow/20',
  }
  const accentClass = accentMap[accent] || accentMap.cyan

  return (
    <div className="card p-5 flex items-start gap-4 animate-slide-up">
      {icon && (
        <div className={`w-10 h-10 rounded-lg border flex items-center justify-center flex-shrink-0 ${accentClass}`}>
          {icon}
        </div>
      )}
      <div className="min-w-0 flex-1">
        <p className="stat-label mb-1">{label}</p>
        <p className="stat-value truncate">{value ?? '—'}</p>
        {sub && <p className="text-xs text-gray-500 mt-0.5 font-body">{sub}</p>}
      </div>
    </div>
  )
}

export function LoadingSpinner({ size = 'md', label = 'Loading...' }) {
  const sizes = { sm: 'w-4 h-4', md: 'w-8 h-8', lg: 'w-12 h-12' }
  return (
    <div className="flex flex-col items-center justify-center gap-3 py-12 text-gray-500">
      <div
        className={`${sizes[size]} border-2 border-bg-border border-t-cyan-glow rounded-full animate-spin`}
      />
      {label && <p className="text-sm font-body">{label}</p>}
    </div>
  )
}

export function PageLoader() {
  return (
    <div className="flex-1 flex items-center justify-center">
      <LoadingSpinner size="lg" label="Loading..." />
    </div>
  )
}

export function ErrorState({ message, onRetry }) {
  return (
    <div className="flex flex-col items-center justify-center gap-4 py-16 text-center">
      <div className="w-12 h-12 rounded-xl bg-red-glow/10 border border-red-glow/20 flex items-center justify-center">
        <svg className="w-6 h-6 text-red-glow" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75}
            d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
        </svg>
      </div>
      <div>
        <p className="text-white font-medium font-display mb-1">Something went wrong</p>
        <p className="text-sm text-gray-500 font-body max-w-xs">{message || 'Failed to load data.'}</p>
      </div>
      {onRetry && (
        <button onClick={onRetry} className="btn-secondary text-sm">
          Try again
        </button>
      )}
    </div>
  )
}

export function EmptyState({ title, description, action }) {
  return (
    <div className="flex flex-col items-center justify-center gap-4 py-16 text-center">
      <div className="w-12 h-12 rounded-xl bg-bg-elevated border border-bg-border flex items-center justify-center">
        <svg className="w-6 h-6 text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5}
            d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
        </svg>
      </div>
      <div>
        <p className="text-white font-medium font-display mb-1">{title}</p>
        {description && <p className="text-sm text-gray-500 font-body max-w-xs">{description}</p>}
      </div>
      {action}
    </div>
  )
}

export function PageHeader({ title, subtitle, action }) {
  return (
    <div className="flex items-start justify-between mb-6">
      <div>
        <h1 className="font-display font-bold text-2xl text-white tracking-wide">{title}</h1>
        {subtitle && <p className="text-sm text-gray-500 mt-0.5 font-body">{subtitle}</p>}
      </div>
      {action && <div className="flex-shrink-0">{action}</div>}
    </div>
  )
}

export function InlineError({ message }) {
  if (!message) return null
  return (
    <div className="flex items-center gap-2 px-3 py-2.5 bg-red-glow/10 border border-red-glow/20 rounded-lg">
      <svg className="w-4 h-4 text-red-glow flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
      </svg>
      <p className="text-sm text-red-glow font-body">{message}</p>
    </div>
  )
}

export function ProgressBar({ value, max, showLabel = false, accent = 'cyan' }) {
  const pct = max > 0 ? Math.min(Math.round((value / max) * 100), 100) : 0
  const fills = {
    cyan: 'from-cyan-dim to-cyan-glow',
    amber: 'from-amber-900 to-amber-glow',
    green: 'from-green-900 to-green-glow',
  }
  return (
    <div className="flex items-center gap-2">
      <div className="progress-bar flex-1">
        <div
          className={`progress-fill bg-gradient-to-r ${fills[accent] || fills.cyan}`}
          style={{ width: `${pct}%` }}
        />
      </div>
      {showLabel && (
        <span className="text-xs text-gray-500 font-mono w-9 text-right">{pct}%</span>
      )}
    </div>
  )
}
