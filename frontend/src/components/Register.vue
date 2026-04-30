<script setup lang="ts">
import { register, type RegisterDto } from '@/services/authService'
import { ref } from 'vue'

const registerValue = ref<RegisterDto>({
  email: '',
  username: '',
  password: '',
})

const message = ref<string | null>(null)
const error = ref<string | null>(null)

const validateRegister = () => {
  if (!registerValue.value.email.trim()) {
    return 'Veuillez entrer un email'
  }
  // TODO : add other rules for email verification

  if (!registerValue.value.username.trim()) {
    return "Veuillez entrer un nom d'utilisateur"
  }

  if (!registerValue.value.password.trim()) {
    return 'Veuillez entrer un mot de passe contenant au moins un caractères sans espaces'
  }
  //TODO : add other rules for password

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
    await register(registerValue.value)
    message.value = 'Compte crée !'

    //reset form
    registerValue.value = {
      email: '',
      username: '',
      password: '',
    }
  } catch (err: any) {
    if (err.response?.data?.errors) {
      const backendErrors = err.response.data.errors
      error.value = Object.values(backendErrors).flat().join(', ')
    } else {
      error.value = 'Erreur lors de la création du compte'
    }
  }
}
</script>

<template>
  <div class="container">
    <h1>Connexion</h1>

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
      <button type="submit">Se connecter</button>
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
