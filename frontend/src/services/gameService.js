import api from './api'

export const gameService = {

  async getProgress() {
    const { data } = await api.get('/game/progress')
    return data
  },

  async saveProgress(gameId, score, level, metadata = ) {
    const { data } = await api.post('/game/save', { gameId, score, level, metadata })
    return data
  },

  async getGames() {
    const { data } = await api.get('/game')
    return data
  },
}
