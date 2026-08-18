export function formatNumber(n) {
  if (n == null) return '—'
  return Number(n).toLocaleString()
}

export function formatDuration(seconds) {
  if (!seconds || seconds < 0) return '0s'
  const h = Math.floor(seconds / 3600)
  const m = Math.floor((seconds % 3600) / 60)
  const s = seconds % 60
  const parts = []
  if (h) parts.push(`${h}h`)
  if (m) parts.push(`${m}m`)
  if (s || !parts.length) parts.push(`${s}s`)
  return parts.join(' ')
}

export function formatDate(dateStr) {
  if (!dateStr) return '—'
  return new Date(dateStr).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}

export function timeAgo(dateStr) {
  if (!dateStr) return '—'
  const diff = Date.now() - new Date(dateStr).getTime()
  const mins = Math.floor(diff / 60000)
  if (mins < 1) return 'just now'
  if (mins < 60) return `${mins}m ago`
  const hours = Math.floor(mins / 60)
  if (hours < 24) return `${hours}h ago`
  const days = Math.floor(hours / 24)
  if (days < 7) return `${days}d ago`
  return formatDate(dateStr)
}

export function truncate(str, limit = 60) {
  if (!str) return ''
  return str.length > limit ? str.slice(0, limit - 1) + '…' : str
}

export function getInitials(username) {
  if (!username) return '??'
  const parts = username.split(/[_\s-]/).filter(Boolean)
  if (parts.length >= 2) return (parts[0][0] + parts[1][0]).toUpperCase()
  return username.slice(0, 2).toUpperCase()
}

export function clamp(value, min, max) {
  return Math.min(Math.max(value, min), max)
}

export function progressPercent(current, max) {
  if (!max || max <= 0) return 0
  return clamp(Math.round((current / max) * 100), 0, 100)
}

export function getErrorMessage(err) {
  return (
    err?.response?.data?.message ||
    err?.response?.data?.title ||
    err?.message ||
    'An unexpected error occurred.'
  )
}
