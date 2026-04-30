import { login, type AuthResponse, type LoginDto } from '@/services/authService'
import { getToken, removeToken, setToken } from '@/services/tokenService'
import { defineStore } from 'pinia'
export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as AuthResponse | null,
    token: getToken() as string | null,
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
  },

  actions: {
    async loginUser(data: LoginDto) {
      const response = await login(data)

      this.user = response.data
      this.token = response.data.token

      setToken(response.data.token)
    },

    logout() {
      ;((this.user = null), (this.token = null), removeToken())
    },
  },
})
