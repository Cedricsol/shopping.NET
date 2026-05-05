<script setup lang="ts">
import { register, type RegisterDto } from '@/services/authService'
import { useAuthStore } from '@/stores/authStore'
import { ref } from 'vue'

const authStore = useAuthStore()

const registerValue = ref<RegisterDto>({
  email: '',
  username: '',
  password: '',
})

const message = ref<string | null>(null)
const errors = ref<Partial<Record<keyof RegisterDto, string | null>>>({})

const validateEmail = (email: string): string | null => {
  const value = email.trim()
  if (!value) {
    return 'Veuillez entrer un email'
  }
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!emailRegex.test(value)) {
    return "Format d'email invalide"
  }
  if (value.length > 255) {
    return "L'email est trop long"
  }
  return null
}

const validateUsername = (username: string): string | null => {
  const value = username.trim()
  if (!value) {
    return "Veuillez entrer un nom d'utilisateur"
  }
  if (value.length < 3 || value.length > 50) {
    return "Le nom d'utilisateur doit contenir entre 3 et 50 caractères"
  }
  const usernameRegex = /^[a-zA-Z0-9_-]+$/
  if (!usernameRegex.test(value)) {
    return "Caractères invalides dans le nom d'utilisateur"
  }
  return null
}

const validatePassword = (password: string): string | null => {
  const value = password.trim()
  if (!value) {
    return 'Veuillez entrer un mot de passe'
  }
  if (value.length < 8) {
    return 'Le mot de passe doit contenir au moins 8 caractères'
  }
  if (!/[A-Z]/.test(value)) {
    return 'Le mot de passe doit contenir une majuscule'
  }
  if (!/[a-z]/.test(value)) {
    return 'Le mot de passe doit contenir une minuscule'
  }
  if (!/[0-9]/.test(value)) {
    return 'Le mot de passe doit contenir un chiffre'
  }
  if (!/[!@#$%^&*/]/.test(value)) {
    return 'Le mot de passe doit contenir un caractère spécial'
  }
  return null
}

const validateRegister = () => {
  const newErrors: typeof errors.value = {}

  const emailError = validateEmail(registerValue.value.email)
  if (emailError) {
    newErrors.email = emailError
  }

  const usernameError = validateUsername(registerValue.value.username)
  if (usernameError) {
    newErrors.username = usernameError
  }

  const passwordError = validatePassword(registerValue.value.password)
  if (passwordError) {
    newErrors.password = passwordError
  }
  errors.value = newErrors
  return Object.keys(newErrors).length === 0
}

const submitRegister = async () => {
  message.value = null
  errors.value = {}

  const isValid = validateRegister()

  if (!isValid) {
    return
  }

  try {
    await authStore.registerUser(registerValue.value)
    message.value = 'Compte crée !'

    //reset form
    registerValue.value = {
      email: '',
      username: '',
      password: '',
    }
  } catch (err: any) {
    errors.value.email = err
  }
}
</script>

<template>
  <div class="container">
    <h1>Créer un compte</h1>

    <form @submit.prevent="submitRegister" class="form">
      <div>
        <input
          data-testid="register-email"
          v-model="registerValue.email"
          type="text"
          placeholder="Adresse email"
        />
        <p v-if="errors.email" class="error">{{ errors.email }}</p>
      </div>
      <div>
        <input
          data-testid="register-username"
          v-model="registerValue.username"
          type="text"
          placeholder="Nom d'utilisateur"
        />
        <p v-if="errors.username" class="error">{{ errors.username }}</p>
      </div>
      <div>
        <input
          data-testid="register-password"
          v-model="registerValue.password"
          type="password"
          placeholder="Mot de passe"
        />
        <p v-if="errors.password" class="error">{{ errors.password }}</p>
      </div>
      <button type="submit">Créer un compte</button>
    </form>
    <p v-if="message" class="success">{{ message }}</p>
  </div>
</template>

<style scoped>
.container {
  padding: 20px;
  max-width: 400px;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

input {
  width: 100%;
  box-sizing: border-box;
  padding: 8px;
  border: 1px solid #ccc;
}

button {
  padding: 10px;
  background-color: #42b883;
  color: white;
  border: none;
  cursor: pointer;
}

.success {
  color: green;
}

.error {
  color: red;
}
</style>
