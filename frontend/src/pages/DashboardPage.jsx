import { useEffect, useState, useCallback } from 'react'
import { useAuth } from '../context/AuthContext'
import { useToast } from '../context/ToastContext'
import { useAsync } from '../hooks/useAsync'
import { gameService } from '../services/gameService'
import { authService } from '../services/authService'
import { sessionService } from '../services/index'
import {
  StatCard, PageHeader, LoadingSpinner, ErrorState, ProgressBar, EmptyState,
} from '../components/ui'
import { formatNumber, formatDate, timeAgo } from '../utils/helpers'

export default function DashboardPage() {
  const { user, refreshProfile } = useAuth()
  const toast = useToast()

  const {
    data: progressData,
    loading: progressLoading,
    error: progressError,
    execute: loadProgress,
  } = useAsync(gameService.getProgress, true)

  const {
    data: profileData,
    loading: profileLoading,
    execute: loadProfile,
  } = useAsync(authService.getProfile, true)

  const [saving, setSaving] = useState(false)
  const [activeSession, setActiveSession] = useState(null)

  const handleSaveProgress = async (game) => {
    setSaving(true)
    try {
      await gameService.saveProgress(game.gameId, game.score, game.level)
      toast.success(`Progress saved for ${game.gameName}`)
      loadProgress()
    } catch (err) {
      toast.error(err?.response?.data?.message || 'Failed to save progress.')
    } finally {
      setSaving(false)
    }
  }

  const handleStartSession = async (gameId, gameName) => {
    try {
      const session = await sessionService.startSession(gameId)
      setActiveSession({ ...session, gameName })
      toast.success(`Session started for ${gameName}`)
    } catch {
      toast.error('Failed to start session.')
    }
  }

  const handleEndSession = async () => {
    if (!activeSession) return
    try {
      await sessionService.endSession(activeSession.sessionId || activeSession.id)
      toast.success('Session ended.')
      setActiveSession(null)
      loadProgress()
    } catch {
      toast.error('Failed to end session.')
    }
  }

  const profile = profileData || 
  const games = Array.isArray(progressData) ? progressData : []

  const totalScore = games.reduce((s, g) => s + (g.score || 0), 0)
  const maxLevel = games.reduce((m, g) => Math.max(m, g.level || 0), 0)
  const gamesPlayed = profile.totalGamesPlayed ?? games.length

  return (
    <div className="space-y-8 animate-fade-in">
      <PageHeader
        title={`Hey, ${user?.username} 👾`}
        subtitle={`Member since ${formatDate(user?.createdAt || profile?.createdAt)}`}
        action={
          activeSession ? (
            <button
              onClick={handleEndSession}
              className="btn-danger flex items-center gap-2 text-sm"
            >
              <span className="w-2 h-2 rounded-full bg-red-glow animate-pulse" />
              End Session
            </button>
          ) : null
        }
      />

      <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
        <StatCard
          label="Total Score"
          value={formatNumber(totalScore)}
          accent="cyan"
          icon={<ScoreIcon />}
        />
        <StatCard
          label="Highest Level"
          value={maxLevel || '—'}
          accent="amber"
          icon={<LevelIcon />}
        />
        <StatCard
          label="Games Played"
          value={formatNumber(gamesPlayed)}
          accent="green"
          icon={<GameIcon />}
        />
        <StatCard
          label="Achievements"
          value={formatNumber(profile.totalAchievements)}
          accent="red"
          icon={<AchievIcon />}
        />
      </div>

      {activeSession && (
        <div className="flex items-center gap-3 px-4 py-3 bg-green-glow/5 border border-green-glow/20 rounded-xl">
          <span className="w-2.5 h-2.5 rounded-full bg-green-glow animate-pulse flex-shrink-0" />
          <p className="text-sm text-green-glow font-body">
            Active session — <span className="font-medium">{activeSession.gameName}</span>
          </p>
          <button onClick={handleEndSession} className="ml-auto text-xs btn-secondary py-1.5 px-3">
            End
          </button>
        </div>
      )}

      <section>
        <div className="flex items-center justify-between mb-4">
          <h2 className="section-title">Game Progress</h2>
          <button
            onClick={() => loadProgress()}
            className="text-xs text-gray-500 hover:text-cyan-glow transition-colors font-body flex items-center gap-1"
          >
            <svg className="w-3 h-3" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2}
                d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
            </svg>
            Refresh
          </button>
        </div>

        {progressLoading ? (
          <LoadingSpinner label="Loading progress…" />
        ) : progressError ? (
          <ErrorState message={progressError} onRetry={loadProgress} />
        ) : games.length === 0 ? (
          <EmptyState
            title="No game data yet"
            description="Start playing a game to track your progress here."
          />
        ) : (
          <div className="grid gap-4">
            {games.map((game) => (
              <GameProgressCard
                key={game.gameId || game.id}
                game={game}
                onSave={() => handleSaveProgress(game)}
                onStartSession={() => handleStartSession(game.gameId || game.id, game.gameName || 'Game')}
                saving={saving}
                hasActiveSession={!!activeSession}
              />
            ))}
          </div>
        )}
      </section>
    </div>
  )
}

function GameProgressCard({ game, onSave, onStartSession, saving, hasActiveSession }) {
  const xpPct = game.xp && game.xpMax ? Math.min((game.xp / game.xpMax) * 100, 100) : 0

  return (
    <div className="card p-5 flex flex-col gap-4 md:flex-row md:items-center hover:border-bg-elevated transition-colors">
      
      <div className="w-10 h-10 rounded-lg bg-bg-elevated border border-bg-border flex items-center justify-center flex-shrink-0">
        <svg className="w-5 h-5 text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5}
            d="M11 4a2 2 0 114 0v1a1 1 0 001 1h3a1 1 0 011 1v3a1 1 0 01-1 1h-1a2 2 0 100 4h1a1 1 0 011 1v3a1 1 0 01-1 1h-3a1 1 0 01-1-1v-1a2 2 0 10-4 0v1a1 1 0 01-1 1H7a1 1 0 01-1-1v-3a1 1 0 00-1-1H4a2 2 0 110-4h1a1 1 0 001-1V7a1 1 0 011-1h3a1 1 0 001-1V4z" />
        </svg>
      </div>

      <div className="flex-1 min-w-0 space-y-2">
        <div className="flex items-center gap-2 flex-wrap">
          <p className="font-display font-semibold text-white">{game.gameName || 'Unknown Game'}</p>
          <span className="badge-cyan text-[10px]">Lv {game.level ?? 0}</span>
        </div>
        <div className="flex items-center gap-4 text-xs text-gray-500 font-mono flex-wrap">
          <span>Score: <span className="text-white">{formatNumber(game.score)}</span></span>
          {game.xp != null && (
            <span>XP: <span className="text-white">{formatNumber(game.xp)}</span></span>
          )}
          {game.lastPlayedAt && (
            <span>Last played: <span className="text-white">{timeAgo(game.lastPlayedAt)}</span></span>
          )}
        </div>
        {game.xp != null && game.xpMax != null && (
          <ProgressBar value={game.xp} max={game.xpMax} showLabel />
        )}
      </div>

      <div className="flex items-center gap-2 flex-shrink-0">
        {!hasActiveSession && (
          <button
            onClick={onStartSession}
            className="btn-secondary text-xs py-1.5 px-3"
          >
            Play
          </button>
        )}
        <button
          onClick={onSave}
          disabled={saving}
          className="btn-primary text-xs py-1.5 px-3 flex items-center gap-1.5"
        >
          {saving ? (
            <div className="w-3 h-3 border border-bg-primary/30 border-t-bg-primary rounded-full animate-spin" />
          ) : (
            <svg className="w-3 h-3" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2}
                d="M8 7H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-3m-1 4l-3 3m0 0l-3-3m3 3V4" />
            </svg>
          )}
          Save
        </button>
      </div>
    </div>
  )
}

const ScoreIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75}
      d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
  </svg>
)
const LevelIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75}
      d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143L13 3z" />
  </svg>
)
const GameIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75}
      d="M11 4a2 2 0 114 0v1a1 1 0 001 1h3a1 1 0 011 1v3a1 1 0 01-1 1h-1a2 2 0 100 4h1a1 1 0 011 1v3a1 1 0 01-1 1h-3a1 1 0 01-1-1v-1a2 2 0 10-4 0v1a1 1 0 01-1 1H7a1 1 0 01-1-1v-3a1 1 0 00-1-1H4a2 2 0 110-4h1a1 1 0 001-1V7a1 1 0 011-1h3a1 1 0 001-1V4z" />
  </svg>
)
const AchievIcon = () => (
  <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.75}
      d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z" />
  </svg>
)
