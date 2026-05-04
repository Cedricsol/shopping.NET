<script setup lang="ts">
import { type LoginDto } from '@/services/authService'
import { useAuthStore } from '@/stores/authStore'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

const loginValue = ref<LoginDto>({
  email: '',
  password: '',
})

const message = ref<string | null>(null)
const error = ref<string | null>(null)

const validateLogin = () => {
  if (!loginValue.value.email.trim()) {
    return 'Veuillez entrer un email'
  }
  // TODO : add other rules for email verification

  if (!loginValue.value.password.trim()) {
    return 'Veuillez entrer un mot de passe contenant au moins un caractères sans espaces'
  }

  return null
}

const submitLogin = async () => {
  message.value = null
  error.value = null

  const validateError = validateLogin()

  if (validateError) {
    error.value = validateError
    loginValue.value.password = ''
    return
  }

  try {
    await authStore.loginUser(loginValue.value)
    message.value = 'Connexion réussie !'

    //reset form
    loginValue.value = {
      email: '',
      password: '',
    }
    router.push('/')
  } catch (err: any) {
    error.value = err
  }
}
</script>

<template>
  <div class="container">
    <h1>Connexion</h1>

    <form @submit.prevent="submitLogin" class="form">
      <input
        data-testid="login-email"
        v-model="loginValue.email"
        type="text"
        placeholder="Adresse email"
      />
      <input
        data-testid="login-password"
        v-model="loginValue.password"
        type="password"
        placeholder="Mot de passe"
      />
      <button type="submit">Se connecter</button>
    </form>
    <p v-if="message" class="success">{{ message }}</p>
    <p v-if="error" class="error">{{ error }}</p>
    <p>Pas encore de compte ?</p>
    <RouterLink to="/register">Créer un compte</RouterLink>
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
