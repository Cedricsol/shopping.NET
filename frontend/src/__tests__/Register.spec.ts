import { useAuthStore } from '@/stores/authStore'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import Register from '@/components/Register.vue'
import { flushPromises, mount } from '@vue/test-utils'

vi.mock('@/stores/authStore', () => ({
  useAuthStore: vi.fn(),
}))

describe('Register.vue', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('should render register form correctly', () => {
    const wrapper = mount(Register)

    expect(wrapper.find('h1').text()).toBe('Créer un compte')
    expect(wrapper.find('[data-testid="register-email"]').exists()).toBeTruthy()
    expect(wrapper.find('[data-testid="register-username"]').exists()).toBeTruthy()
    expect(wrapper.find('[data-testid="register-password"]').exists()).toBeTruthy()
    expect(wrapper.find('button').text()).toBe('Créer un compte')
  })

  it('should show error when email is empty', async () => {
    const wrapper = mount(Register)

    await wrapper.find('[data-testid="register-username"]').setValue('test')
    await wrapper.find('[data-testid="register-password"]').setValue('test')

    await wrapper.find('form').trigger('submit.prevent')

    expect(wrapper.text()).toContain('Veuillez entrer un email')
  })

  it('should show error when username is empty', async () => {
    const wrapper = mount(Register)

    await wrapper.find('[data-testid="register-email"]').setValue('test@test.com')
    await wrapper.find('[data-testid="register-password"]').setValue('test')

    await wrapper.find('form').trigger('submit.prevent')

    expect(wrapper.text()).toContain("Veuillez entrer un nom d'utilisateur")
  })

  it('should show error when password is empty', async () => {
    const wrapper = mount(Register)

    await wrapper.find('[data-testid="register-email"]').setValue('test@test.com')
    await wrapper.find('[data-testid="register-username"]').setValue('test')

    await wrapper.find('form').trigger('submit.prevent')

    expect(wrapper.text()).toContain('Veuillez entrer un mot de passe')
  })

  it('should register successfully', async () => {
    const registerMock = vi.fn().mockResolvedValue(undefined)

    vi.mocked(useAuthStore).mockReturnValue({
      registerUser: registerMock,
    } as any)

    const wrapper = mount(Register)

    await wrapper.find('[data-testid="register-email"]').setValue('test@test.com')
    await wrapper.find('[data-testid="register-username"]').setValue('test')
    await wrapper.find('[data-testid="register-password"]').setValue('test')

    await wrapper.find('form').trigger('submit.prevent')
    await flushPromises()

    expect(registerMock).toHaveBeenCalledWith({
      email: 'test@test.com',
      username: 'test',
      password: 'test',
    })
    expect(wrapper.text()).toContain('Compte crée !')
  })

  it('should display backend error', async () => {
    const registerMock = vi.fn().mockRejectedValue('Erreur serveur')

    vi.mocked(useAuthStore).mockReturnValue({
      registerUser: registerMock,
    } as any)

    const wrapper = mount(Register)

    await wrapper.find('[data-testid="register-email"]').setValue('test@test.com')
    await wrapper.find('[data-testid="register-username"]').setValue('test')
    await wrapper.find('[data-testid="register-password"]').setValue('wrong password')

    await wrapper.find('form').trigger('submit.prevent')
    await flushPromises()

    expect(wrapper.text()).toContain('Erreur serveur')
  })

  it('should reset form after success', async () => {
    const registerMock = vi.fn().mockResolvedValue(undefined)

    vi.mocked(useAuthStore).mockReturnValue({
      registerUser: registerMock,
    } as any)

    const wrapper = mount(Register)

    const email = wrapper.find('[data-testid="register-email"]')
    const username = wrapper.find('[data-testid="register-username"]')
    const password = wrapper.find('[data-testid="register-password"]')

    await email.setValue('test@test.com')
    await username.setValue('test')
    await password.setValue('test')

    await wrapper.find('form').trigger('submit.prevent')
    await flushPromises()

    expect((email.element as HTMLInputElement).value).toBe('')
    expect((username.element as HTMLInputElement).value).toBe('')
    expect((password.element as HTMLInputElement).value).toBe('')
  })
})
