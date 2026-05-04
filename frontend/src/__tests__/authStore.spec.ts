import { beforeEach, describe, expect, it, vi } from 'vitest'
import { useAuthStore } from '@/stores/authStore'
import * as authService from '@/services/authService'
import * as tokenService from '@/services/tokenService'
import { isAxiosError, type InternalAxiosRequestConfig } from 'axios'
import { createPinia, setActivePinia } from 'pinia'

vi.mock('@/services/authService', () => ({
  login: vi.fn(),
}))

describe('authStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
  })

  it('should login user and store token', async () => {
    const store = useAuthStore()

    const mockResponse = {
      data: {
        token: 'token',
        email: 'test@test.com',
        username: 'test',
        role: 0,
      },
      status: 200,
      statusText: 'OK',
      headers: {},
      config: {
        headers: {},
      } as InternalAxiosRequestConfig,
    }
    vi.spyOn(authService, 'login').mockResolvedValueOnce(mockResponse)

    const setTokenSpy = vi.spyOn(tokenService, 'setToken')

    await store.loginUser({
      email: 'test@test.com',
      password: 'test',
    })

    expect(store.user).toEqual(mockResponse.data)
    expect(store.token).toBe('token')
    expect(setTokenSpy).toHaveBeenCalledWith('token')
  })

  it('should throw formatted backend errors on login', async () => {
    const store = useAuthStore()

    vi.spyOn(authService, 'login').mockRejectedValue({
      isAxiosError: true,
      response: {
        data: {
          errors: {
            email: ['Email invalide'],
            password: ['Mot de passe requis'],
          },
        },
      },
    })
    await expect(store.loginUser({ email: '', password: '' })).rejects.toBe(
      'Email invalide, Mot de passe requis',
    )
  })

  it('should throw generic error when login fails', async () => {
    const store = useAuthStore()

    vi.spyOn(authService, 'login').mockRejectedValue(new Error())

    await expect(store.loginUser({ email: 'test', password: 'test' })).rejects.toBe(
      'Erreur lors de la connexion',
    )
  })

  it('should clear user and token on logout', () => {
    const store = useAuthStore()

    store.user = { token: 'token', email: '', username: '', role: 0 }
    store.token = 'token'

    const removeTokenSpy = vi.spyOn(tokenService, 'removeToken')

    store.logout()

    expect(store.user).toBeNull()
    expect(store.token).toBeNull()
    expect(removeTokenSpy).toHaveBeenCalledOnce()
  })

  it('should return true when token exists', () => {
    const store = useAuthStore()
    store.token = 'token'

    expect(store.isAuthenticated).toBeTruthy()
  })

  it('should return false when no token', () => {
    const store = useAuthStore()
    store.token = null

    expect(store.isAuthenticated).toBeFalsy()
  })
})
