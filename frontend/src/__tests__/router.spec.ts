import { UserRole } from '@/services/authService'
import { useAuthStore } from '@/stores/authStore'
import { createPinia, setActivePinia } from 'pinia'
import { beforeEach, describe, expect, it } from 'vitest'
import router from '@/router'

describe('authorization router guard', () => {
  beforeEach(async () => {
    setActivePinia(createPinia())

    await router.push('/')
  })

  it('redirects non-admin user from addProduct page', async () => {
    const authStore = useAuthStore()

    authStore.token = 'token'
    authStore.role = UserRole.User

    await router.push('/addProduct')

    expect(router.currentRoute.value.fullPath).toBe('/login')
  })

  it('allows admin user to access addProduct page', async () => {
    const authStore = useAuthStore()

    authStore.token = 'token'
    authStore.role = UserRole.Admin

    await router.push('/addProduct')

    expect(router.currentRoute.value.fullPath).toBe('/addProduct')
  })

  it('redirects unautenticated user to login', async () => {
    const authStore = useAuthStore()

    authStore.token = null
    authStore.role = null

    await router.push('/addProduct')

    expect(router.currentRoute.value.fullPath).toBe('/login')
  })
})
