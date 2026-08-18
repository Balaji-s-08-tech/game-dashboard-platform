import { useAsync } from '../hooks/useAsync'
import { sessionService } from '../services/index'
import {
  PageHeader, LoadingSpinner, ErrorState, EmptyState, StatCard,
} from '../components/ui'
import { formatDate, formatDuration, timeAgo, formatNumber } from '../utils/helpers'

export default function SessionsPage() {
  const {
    data,
    loading,
    error,
    execute: reload,
  } = useAsync(sessionService.getSessions, true)

  const sessions = Array.isArray(data) ? data : []

  const totalTime = sessions.reduce((s, sess) => s + (sess.durationSeconds || 0), 0)
  const totalSessions = sessions.length
  const avgTime = totalSessions > 0 ? Math.round(totalTime / totalSessions) : 0
  const longestSession = sessions.reduce((m, s) => Math.max(m, s.durationSeconds || 0), 0)

  return (
    <div className="space-y-6 animate-fade-in">
      <PageHeader
        title="Sessions"
        subtitle="Your play session history"
        action={
          <button onClick={reload} className="btn-secondary text-sm flex items-center gap-2">
            <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2}
                d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
            </svg>
            Refresh
          </button>
        }
      />

      {!loading && sessions.length > 0 && (
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          <StatCard
            label="Total Playtime"
            value={formatDuration(totalTime)}
            accent="cyan"
            icon={<ClockIcon />}
          />
          <StatCard
            label="Total Sessions"
            value={formatNumber(totalSessions)}
            accent="amber"
            icon={<ListIcon />}
          />
          <StatCard
            label="Avg Session"
            value={formatDuration(avgTime)}
            accent="green"
            icon={<AvgIcon />}
          />
          <StatCard
            label="Longest Session"
            value={formatDuration(longestSession)}
            accent="red"
            icon={<TrophyIcon />}
          />
        </div>
      )}

      <div className="card overflow-hidden">
        <div className="px-5 py-4 border-b border-bg-border flex items-center justify-between">
          <h2 className="section-title">Session History</h2>
          {!loading && sessions.length > 0 && (
            <span className="text-xs text-gray-500 font-mono">{sessions.length} sessions</span>
          )}
        </div>

        {loading ? (
          <LoadingSpinner label="Loading sessions…" />
        ) : error ? (
          <ErrorState message={error} onRetry={reload} />
        ) : sessions.length === 0 ? (
          <EmptyState
            title="No sessions yet"
            description="Start a session from the Dashboard to track your playtime."
          />
        ) : (
          <div className="divide-y divide-bg-border">
            {sessions.map((session, idx) => (
              <SessionRow key={session.id || idx} session={session} />
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

function SessionRow({ session }) {
  const isActive = !session.endedAt && !session.endTime

  return (
    <div className="flex items-center gap-4 px-5 py-4 hover:bg-bg-elevated transition-colors">
      
      <div className="flex-shrink-0">
        {isActive ? (
          <span className="w-2.5 h-2.5 rounded-full bg-green-glow animate-pulse block" />
        ) : (
          <span className="w-2.5 h-2.5 rounded-full bg-bg-border block" />
        )}
      </div>

      <div className="flex-1 min-w-0">
        <div className="flex items-center gap-2 flex-wrap">
          <p className="text-sm font-medium text-white font-body">
            {session.gameName || 'Unknown Game'}
          </p>
          {isActive && <span className="badge-green text-[10px]">Active</span>}
        </div>
        <p className="text-xs text-gray-600 font-mono mt-0.5">
          {formatDate(session.startedAt || session.startTime)}
          {' · '}
          {timeAgo(session.startedAt || session.startTime)}
        </p>
      </div>

      <div className="text-right flex-shrink-0">
        <p className="text-sm font-mono text-white">
          {isActive ? (
            <span className="text-green-glow">Ongoing</span>
          ) : (
            formatDuration(session.durationSeconds)
          )}
        </p>
        {!isActive && session.endedAt && (
          <p className="text-xs text-gray-600 font-body">
            Ended {timeAgo(session.endedAt || session.endTime)}
          </p>
        )}
      </div>
    </div>
  )
}

const ClockIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
  </svg>
)
const ListIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75} d="M4 6h16M4 10h16M4 14h16M4 18h16" />
  </svg>
)
const AvgIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75} d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
  </svg>
)
const TrophyIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75} d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143L13 3z" />
  </svg>
)
