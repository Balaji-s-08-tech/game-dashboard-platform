import { createContext, useContext, useState, useCallback, useRef } from 'react'

const ToastContext = createContext(null)

let idCounter = 0

export function ToastProvider({ children }) {
  const [toasts, setToasts] = useState([])

  const remove = useCallback((id) => {
    setToasts((prev) => prev.filter((t) => t.id !== id))
  }, [])

  const add = useCallback((message, type = 'info', duration = 4000) => {
    const id = ++idCounter
    setToasts((prev) => [...prev.slice(-4), { id, message, type }])

    if (duration > 0) {
      setTimeout(() => remove(id), duration)
    }
    return id
  }, [remove])

  const toast = {
    success: (msg, duration) => add(msg, 'success', duration),
    error: (msg, duration) => add(msg, 'error', duration),
    info: (msg, duration) => add(msg, 'info', duration),
    warning: (msg, duration) => add(msg, 'warning', duration),
  }

  return (
    <ToastContext.Provider value={{ toast, toasts, remove }}>
      {children}
      <ToastContainer toasts={toasts} remove={remove} />
    </ToastContext.Provider>
  )
}

export function useToast() {
  const ctx = useContext(ToastContext)
  if (!ctx) throw new Error('useToast must be used within a ToastProvider')
  return ctx.toast
}

const TYPE_STYLES = {
  success: {
    border: 'border-green-glow/30',
    icon: (
      <svg className="w-4 h-4 text-green-glow flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
      </svg>
    ),
    label: 'text-green-glow',
  },
  error: {
    border: 'border-red-glow/30',
    icon: (
      <svg className="w-4 h-4 text-red-glow flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
      </svg>
    ),
    label: 'text-red-glow',
  },
  warning: {
    border: 'border-amber-glow/30',
    icon: (
      <svg className="w-4 h-4 text-amber-glow flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z" />
      </svg>
    ),
    label: 'text-amber-glow',
  },
  info: {
    border: 'border-cyan-glow/30',
    icon: (
      <svg className="w-4 h-4 text-cyan-glow flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
    ),
    label: 'text-cyan-glow',
  },
}

function ToastContainer({ toasts, remove }) {
  return (
    <div className="fixed top-4 right-4 z-50 flex flex-col gap-2 w-80">
      {toasts.map((t) => {
        const styles = TYPE_STYLES[t.type] || TYPE_STYLES.info
        return (
          <div
            key={t.id}
            className={`animate-slide-in-right bg-bg-elevated border ${styles.border} rounded-xl p-3.5 shadow-card flex items-start gap-3`}
          >
            {styles.icon}
            <span className="text-sm text-gray-200 flex-1 font-body leading-snug">{t.message}</span>
            <button
              onClick={() => remove(t.id)}
              className="text-gray-500 hover:text-white transition-colors flex-shrink-0 mt-0.5"
            >
              <svg className="w-3.5 h-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        )
      })}
    </div>
  )
}
