import {
  login,
  register,
  UserRole,
  type AuthResponse,
  type LoginDto,
  type RegisterDto,
} from '@/services/authService'
import {
  getRole,
  getToken,
  removeRole,
  removeToken,
  setRole,
  setToken,
} from '@/services/tokenService'
import axios from 'axios'
import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as AuthResponse | null,
    token: getToken() as string | null,
    role: getRole() as UserRole | null,
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,

    isAdmin: (state) => state.role === UserRole.Admin,
  },

  actions: {
    async loginUser(data: LoginDto) {
      try {
        const response = await login(data)

        this.user = response.data
        this.token = response.data.token
        this.role = response.data.role

        setToken(this.token)
        setRole(this.role.toString())
      } catch (err: unknown) {
        if (axios.isAxiosError(err)) {
          const backendErrors = err.response?.data?.errors
          if (backendErrors) {
            throw Object.values(backendErrors).flat().join(', ')
          }
        }
        throw 'Erreur lors de la connexion'
      }
    },

    async registerUser(data: RegisterDto) {
      try {
        await register(data)
      } catch (err: unknown) {
        if (axios.isAxiosError(err)) {
          const backendErrors = err.response?.data?.errors
          if (backendErrors) {
            throw Object.values(backendErrors).flat().join(', ')
          }
        }
        throw 'Erreur lors de la création du compte'
      }
    },

    logout() {
      this.user = null
      this.token = null
      this.role = null
      removeToken()
      removeRole()
    },
  },
})
