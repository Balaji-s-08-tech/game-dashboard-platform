import { useState } from 'react'
import { useAsync } from '../hooks/useAsync'
import { achievementService } from '../services/index'
import {
  PageHeader, LoadingSpinner, ErrorState, EmptyState, ProgressBar,
} from '../components/ui'
import { formatDate } from '../utils/helpers'

const FILTERS = ['All', 'Unlocked', 'Locked']

export default function AchievementsPage() {
  const [filter, setFilter] = useState('All')

  const {
    data,
    loading,
    error,
    execute: reload,
  } = useAsync(achievementService.getAchievements, true)

  const all = Array.isArray(data) ? data : []
  const unlocked = all.filter((a) => a.isUnlocked || a.unlockedAt)
  const locked = all.filter((a) => !a.isUnlocked && !a.unlockedAt)

  const filtered =
    filter === 'Unlocked' ? unlocked
    : filter === 'Locked' ? locked
    : all

  const pct = all.length > 0 ? Math.round((unlocked.length / all.length) * 100) : 0

  return (
    <div className="space-y-6 animate-fade-in">
      <PageHeader
        title="Achievements"
        subtitle={`${unlocked.length} of ${all.length} unlocked`}
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

      {!loading && all.length > 0 && (
        <div className="card p-5 flex items-center gap-5">
          <div className="flex-1 space-y-2">
            <div className="flex items-center justify-between">
              <p className="text-sm text-gray-400 font-body">Overall completion</p>
              <p className="text-sm font-mono text-cyan-glow">{pct}%</p>
            </div>
            <ProgressBar value={unlocked.length} max={all.length} />
          </div>
          <div className="flex gap-4 flex-shrink-0">
            <MiniStat label="Unlocked" value={unlocked.length} color="text-green-glow" />
            <MiniStat label="Locked" value={locked.length} color="text-gray-500" />
          </div>
        </div>
      )}

      <div className="flex gap-1 p-1 bg-bg-secondary rounded-lg w-fit border border-bg-border">
        {FILTERS.map((f) => (
          <button
            key={f}
            onClick={() => setFilter(f)}
            className={`px-4 py-1.5 rounded-md text-sm font-medium font-body transition-all ${
              filter === f
                ? 'bg-bg-card text-white shadow-sm border border-bg-border'
                : 'text-gray-500 hover:text-gray-300'
            }`}
          >
            {f}
            {f !== 'All' && !loading && (
              <span className="ml-1.5 text-xs opacity-60">
                {f === 'Unlocked' ? unlocked.length : locked.length}
              </span>
            )}
          </button>
        ))}
      </div>

      {loading ? (
        <LoadingSpinner label="Loading achievements…" />
      ) : error ? (
        <ErrorState message={error} onRetry={reload} />
      ) : filtered.length === 0 ? (
        <EmptyState
          title={filter === 'Unlocked' ? 'No achievements unlocked yet' : 'No achievements found'}
          description={filter === 'Unlocked' ? 'Keep playing to earn achievements.' : undefined}
        />
      ) : (
        <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {filtered.map((achievement) => (
            <AchievementCard key={achievement.id || achievement.achievementId} achievement={achievement} />
          ))}
        </div>
      )}
    </div>
  )
}

function AchievementCard({ achievement }) {
  const isUnlocked = achievement.isUnlocked || !!achievement.unlockedAt

  return (
    <div
      className={`card p-5 flex gap-4 transition-all duration-200 ${
        isUnlocked
          ? 'border-bg-border hover:border-amber-glow/30 hover:shadow-amber-glow'
          : 'opacity-50 hover:opacity-70'
      }`}
    >
      
      <div
        className={`w-12 h-12 rounded-xl flex items-center justify-center flex-shrink-0 text-xl border ${
          isUnlocked
            ? 'bg-amber-glow/10 border-amber-glow/20'
            : 'bg-bg-elevated border-bg-border'
        }`}
      >
        {isUnlocked ? (achievement.icon || '🏆') : '🔒'}
      </div>

      <div className="min-w-0 flex-1 space-y-1">
        <div className="flex items-start justify-between gap-2">
          <p className={`font-display font-semibold text-sm ${isUnlocked ? 'text-white' : 'text-gray-500'}`}>
            {achievement.name || achievement.title}
          </p>
          {isUnlocked && <span className="badge-amber text-[10px] flex-shrink-0">Unlocked</span>}
        </div>

        <p className="text-xs text-gray-500 font-body leading-relaxed">
          {achievement.description || 'No description available.'}
        </p>

        {isUnlocked && achievement.unlockedAt && (
          <p className="text-[10px] text-gray-600 font-mono pt-1">
            {formatDate(achievement.unlockedAt)}
          </p>
        )}

        {achievement.points != null && (
          <div className="flex items-center gap-1 pt-1">
            <svg className="w-3 h-3 text-amber-glow" fill="currentColor" viewBox="0 0 20 20">
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"/>
            </svg>
            <span className={`text-[10px] font-mono ${isUnlocked ? 'text-amber-glow' : 'text-gray-600'}`}>
              {achievement.points} pts
            </span>
          </div>
        )}
      </div>
    </div>
  )
}

function MiniStat({ label, value, color }) {
  return (
    <div className="text-center">
      <p className={`text-lg font-mono font-semibold ${color}`}>{value}</p>
      <p className="text-[10px] text-gray-600 uppercase tracking-wider font-body">{label}</p>
    </div>
  )
}
