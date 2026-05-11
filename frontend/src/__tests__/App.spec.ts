import router from '@/router'
import { UserRole } from '@/services/authService'
import { useAuthStore } from '@/stores/authStore'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { beforeEach, describe, expect, it } from 'vitest'
import App from '@/App.vue'

describe('App.vue', () => {
  beforeEach(async () => {
    setActivePinia(createPinia())

    await router.push('/')
  })

  it('shows add product link for admin', () => {
    const authStore = useAuthStore()

    authStore.role = UserRole.Admin
    authStore.token = 'token'

    const wrapper = mount(App, {
      global: {
        plugins: [router],
      },
    })

    expect(wrapper.text()).toContain('Ajouter un produit à la boutique')
  })

  it('hides add product link for non-admin', () => {
    const authStore = useAuthStore()

    authStore.role = UserRole.User
    authStore.token = 'token'

    const wrapper = mount(App, {
      global: {
        plugins: [router],
      },
    })

    expect(wrapper.text()).not.toContain('Ajouter un produit à la boutique')
  })

  it('hides add product link for non-admin', () => {
    const authStore = useAuthStore()

    authStore.role = null
    authStore.token = null

    const wrapper = mount(App, {
      global: {
        plugins: [router],
      },
    })

    expect(wrapper.text()).not.toContain('Ajouter un produit à la boutique')
  })
})
