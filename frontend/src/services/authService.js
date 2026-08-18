import api from './api'

export const authService = {

  async login(email, password) {
    const { data } = await api.post('/auth/login', { email, password })
    return data
  },

  async register(username, email, password) {
    const { data } = await api.post('/auth/register', { username, email, password })
    return data
  },

  async getProfile() {
    const { data } = await api.get('/auth/profile')
    return data
  },
}
