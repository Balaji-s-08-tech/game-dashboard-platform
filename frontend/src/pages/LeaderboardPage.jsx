import { useAuth } from '../context/AuthContext'
import { useAsync } from '../hooks/useAsync'
import { leaderboardService } from '../services/index'
import { PageHeader, LoadingSpinner, ErrorState, EmptyState } from '../components/ui'
import { formatNumber, getInitials } from '../utils/helpers'

export default function LeaderboardPage() {
  const { user } = useAuth()

  const {
    data,
    loading,
    error,
    execute: reload,
  } = useAsync(leaderboardService.getLeaderboard, true)

  const entries = Array.isArray(data) ? data.slice(0, 10) : []

  return (
    <div className="space-y-6 animate-fade-in">
      <PageHeader
        title="Leaderboard"
        subtitle="Top 10 players across all games"
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

      {!loading && !error && entries.length >= 3 && (
        <Podium top3={entries.slice(0, 3)} currentUserId={user?.id} />
      )}

      <div className="card overflow-hidden">
        <div className="px-5 py-4 border-b border-bg-border flex items-center justify-between">
          <h2 className="section-title">Rankings</h2>
          {!loading && entries.length > 0 && (
            <span className="text-xs text-gray-500 font-mono">{entries.length} players</span>
          )}
        </div>

        {loading ? (
          <LoadingSpinner label="Loading leaderboard…" />
        ) : error ? (
          <ErrorState message={error} onRetry={reload} />
        ) : entries.length === 0 ? (
          <EmptyState title="No rankings yet" description="Be the first to set a score!" />
        ) : (
          <div className="divide-y divide-bg-border">
            {entries.map((entry, idx) => {
              const isMe = String(entry.userId) === String(user?.id) ||
                           entry.username === user?.username
              return (
                <LeaderboardRow
                  key={entry.userId || idx}
                  entry={entry}
                  rank={idx + 1}
                  isMe={isMe}
                />
              )
            })}
          </div>
        )}
      </div>
    </div>
  )
}

function Podium({ top3, currentUserId }) {

  const order = [top3[1], top3[0], top3[2]]
  const heights = ['h-20', 'h-28', 'h-16']
  const ranks = [2, 1, 3]

  return (
    <div className="card p-6">
      <h2 className="section-title mb-6 text-center">Top Players</h2>
      <div className="flex items-end justify-center gap-3">
        {order.map((entry, i) => {
          if (!entry) return <div key={i} className="w-28" />
          const isMe = String(entry.userId) === String(currentUserId) ||
                       entry.username === currentUserId?.username
          return (
            <PodiumSlot
              key={entry.userId}
              entry={entry}
              rank={ranks[i]}
              barHeight={heights[i]}
              isMe={isMe}
            />
          )
        })}
      </div>
    </div>
  )
}

function PodiumSlot({ entry, rank, barHeight, isMe }) {
  const rankColors = { 1: '#ffd700', 2: '#c0c0c0', 3: '#cd7f32' }
  const color = rankColors[rank]
  const label = { 1: '1st', 2: '2nd', 3: '3rd' }[rank]

  return (
    <div className="flex flex-col items-center gap-2 w-28">
      
      <div
        className={`w-12 h-12 rounded-full flex items-center justify-center border-2 text-sm font-bold font-mono flex-shrink-0 ${
          isMe
            ? 'border-cyan-glow bg-cyan-glow/10 text-cyan-glow'
            : 'border-bg-border bg-bg-elevated text-gray-400'
        }`}
      >
        {getInitials(entry.username)}
      </div>

      <p className="text-xs font-medium text-white text-center truncate w-full font-body">
        {entry.username}
        {isMe && <span className="text-cyan-glow"> (you)</span>}
      </p>
      <p className="text-xs text-gray-500 font-mono">{formatNumber(entry.totalScore)}</p>

      <div
        className={`w-full ${barHeight} rounded-t-lg flex items-center justify-center`}
        style={{ backgroundColor: `${color}15`, border: `1px solid ${color}30` }}
      >
        <span className="font-display font-bold text-sm" style={{ color }}>
          {label}
        </span>
      </div>
    </div>
  )
}

function LeaderboardRow({ entry, rank, isMe }) {
  const rankLabel = { 1: '🥇', 2: '🥈', 3: '🥉' }[rank] || null

  return (
    <div
      className={`flex items-center gap-4 px-5 py-3.5 transition-colors ${
        isMe
          ? 'bg-cyan-glow/5 border-l-2 border-cyan-glow'
          : 'hover:bg-bg-elevated'
      }`}
    >
      
      <div className="w-8 text-center flex-shrink-0">
        {rankLabel ? (
          <span className="text-base">{rankLabel}</span>
        ) : (
          <span className="text-sm text-gray-500 font-mono">#{rank}</span>
        )}
      </div>

      <div
        className={`w-8 h-8 rounded-full flex items-center justify-center text-xs font-bold font-mono flex-shrink-0 ${
          isMe
            ? 'bg-cyan-glow/10 border border-cyan-glow/30 text-cyan-glow'
            : 'bg-bg-elevated border border-bg-border text-gray-500'
        }`}
      >
        {getInitials(entry.username)}
      </div>

      <div className="flex-1 min-w-0">
        <p className={`text-sm font-medium truncate font-body ${isMe ? 'text-cyan-glow' : 'text-white'}`}>
          {entry.username}
          {isMe && <span className="text-xs text-gray-500 ml-1.5">(you)</span>}
        </p>
        {entry.gameName && (
          <p className="text-xs text-gray-600 truncate font-body">{entry.gameName}</p>
        )}
      </div>

      <div className="flex items-center gap-6 flex-shrink-0">
        {entry.level != null && (
          <div className="text-right hidden sm:block">
            <p className="text-xs text-gray-600 font-body">Level</p>
            <p className="text-sm font-mono text-white">{entry.level}</p>
          </div>
        )}
        <div className="text-right">
          <p className="text-xs text-gray-600 font-body">Score</p>
          <p className={`text-sm font-mono font-semibold ${isMe ? 'text-cyan-glow' : 'text-white'}`}>
            {formatNumber(entry.totalScore ?? entry.score)}
          </p>
        </div>
      </div>
    </div>
  )
}
