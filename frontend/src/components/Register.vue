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
const error = ref<string | null>(null)

const validateRegister = () => {
  const email = registerValue.value.email.trim()
  if (!email) {
    return 'Veuillez entrer un email'
  }
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!emailRegex.test(email)) {
    return "Format d'email invalide"
  }
  if (email.length > 255) {
    return "L'email est trop long"
  }

  const username = registerValue.value.username.trim()
  if (!username) {
    return "Veuillez entrer un nom d'utilisateur"
  }
  if (username.length < 3 || username.length > 50) {
    return "Le nom d'utilisateur doit contenir entre 3 et 50 caractères"
  }
  const usernameRegex = /^[a-zA-Z0-9_-]+$/
  if (!usernameRegex.test(username)) {
    return "Caractères invalides dans le nom d'utilisateur"
  }

  const password = registerValue.value.password.trim()
  if (!password) {
    return 'Veuillez entrer un mot de passe'
  }
  if (password.length < 8) {
    return 'Le mot de passe doit contenir au moins 8 caractères'
  }
  if (!/[A-Z]/.test(password)) {
    return 'Le mot de passe doit contenir une majuscule'
  }
  if (!/[a-z]/.test(password)) {
    return 'Le mot de passe doit contenir une minuscule'
  }
  if (!/[0-9]/.test(password)) {
    return 'Le mot de passe doit contenir un chiffre'
  }
  if (!/[!@#$%^&*/]/.test(password)) {
    return 'Le mot de passe doit contenir un caractère spécial'
  }

  return null
}

const submitRegister = async () => {
  message.value = null
  error.value = null

  const validateError = validateRegister()

  if (validateError) {
    error.value = validateError
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
    error.value = err
  }
}
</script>

<template>
  <div class="container">
    <h1>Créer un compte</h1>

    <form @submit.prevent="submitRegister" class="form">
      <input
        data-testid="register-email"
        v-model="registerValue.email"
        type="text"
        placeholder="Adresse email"
      />
      <input
        data-testid="register-username"
        v-model="registerValue.username"
        type="text"
        placeholder="Nom d'utilisateur"
      />
      <input
        data-testid="register-password"
        v-model="registerValue.password"
        type="password"
        placeholder="Mot de passe"
      />
      <button type="submit">Créer un compte</button>
    </form>
    <p v-if="message" class="success">{{ message }}</p>
    <p v-if="error" class="error">{{ error }}</p>
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
