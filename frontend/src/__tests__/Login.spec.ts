import Login from '@/components/Login.vue'
import { useAuthStore } from '@/stores/authStore'
import { flushPromises, mount } from '@vue/test-utils'
import { describe, expect, it, vi } from 'vitest'

vi.mock('@/stores/authStore', () => ({
  useAuthStore: vi.fn(),
}))

const pushMock = vi.fn()
vi.mock('vue-router', () => ({
  useRouter: () => ({
    push: pushMock,
  }),
}))

describe('Login.vue', () => {
  it('should render login form correctly', () => {
    const wrapper = mount(Login)

    expect(wrapper.find('h1').text()).toBe('Connexion')
    expect(wrapper.find('[data-testid="login-email"]').exists()).toBeTruthy()
    expect(wrapper.find('[data-testid="login-password"]').exists()).toBeTruthy()
    expect(wrapper.find('button').text()).toBe('Se connecter')
  })

  it('should show error when email is empty', async () => {
    const wrapper = mount(Login)

    await wrapper.find('[data-testid="login-password"]').setValue('test')
    await wrapper.find('form').trigger('submit.prevent')

    expect(wrapper.text()).toContain('Veuillez entrer un email')
  })

  it('should show error when password is empty', async () => {
    const wrapper = mount(Login)

    await wrapper.find('[data-testid="login-email"]').setValue('test@test.com')
    await wrapper.find('form').trigger('submit.prevent')

    expect(wrapper.text()).toContain('Veuillez entrer un mot de passe')
  })

  it('should login successfully and redirect', async () => {
    const loginMock = vi.fn().mockResolvedValue(undefined)

    vi.mocked(useAuthStore).mockReturnValue({
      loginUser: loginMock,
    } as any)

    const wrapper = mount(Login)

    await wrapper.find('[data-testid="login-email"]').setValue('test@test.com')
    await wrapper.find('[data-testid="login-password"]').setValue('test')

    await wrapper.find('form').trigger('submit.prevent')
    await flushPromises()

    expect(loginMock).toHaveBeenCalledWith({
      email: 'test@test.com',
      password: 'test',
    })

    expect(wrapper.text()).toContain('Connexion réussie !')
    expect(pushMock).toHaveBeenCalledWith('/')
  })

  it('should display backend error', async () => {
    const loginMock = vi.fn().mockRejectedValue('Erreur serveur')

    vi.mocked(useAuthStore).mockReturnValue({
      loginUser: loginMock,
    } as any)

    const wrapper = mount(Login)

    await wrapper.find('[data-testid="login-email"]').setValue('test@test.com')
    await wrapper.find('[data-testid="login-password').setValue('wrong password')

    await wrapper.find('form').trigger('submit.prevent')
    await flushPromises()

    expect(wrapper.text()).toContain('Erreur serveur')
  })

  it('should reset form after success', async () => {
    const loginMock = vi.fn().mockResolvedValue(undefined)

    vi.mocked(useAuthStore).mockReturnValue({
      loginUser: loginMock,
    } as any)

    const wrapper = mount(Login)

    const email = wrapper.find('[data-testid="login-email"]')
    const password = wrapper.find('[data-testid="login-password"]')

    await email.setValue('test@test.com')
    await password.setValue('test')

    await wrapper.find('form').trigger('submit.prevent')
    await flushPromises()

    expect((email.element as HTMLInputElement).value).toBe('')
    expect((password.element as HTMLInputElement).value).toBe('')
  })
})
