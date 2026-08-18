import api from './api'

export const leaderboardService = {
  async getLeaderboard() {
    const { data } = await api.get('/leaderboard')
    return data
  },
}

export const achievementService = {

  async getAchievements() {
    const { data } = await api.get('/achievement')
    return data
  },

  async unlockAchievement(achievementId) {
    const { data } = await api.post('/achievement/unlock', { achievementId })
    return data
  },
}

export const sessionService = {

  async startSession(gameId) {
    const { data } = await api.post('/session/start', { gameId })
    return data
  },

  async endSession(sessionId) {
    const { data } = await api.post('/session/end', { sessionId })
    return data
  },

  async getSessions() {
    const { data } = await api.get('/session')
    return data
  },
}
