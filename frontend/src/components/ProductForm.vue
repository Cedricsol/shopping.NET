<script setup lang="ts">
import { postProduct, type Product } from '@/services/productService'
import axios from 'axios'
import { ref } from 'vue'

const product = ref<Product>({
  name: '',
  price: 0,
  imageUrl: '',
})

const message = ref<string | null>(null)
const error = ref<string | null>(null)

const validateProduct = () => {
  if (!product.value.name.trim()) {
    return 'Veuillez entrer un nom'
  }

  if (product.value.price < 0) {
    product.value.price = -product.value.price
    return 'Le prix ne doit pas être négatif'
  }

  if (!product.value.imageUrl.trim()) {
    return "Veuillez entrer un chemin d'accès pour l'image sous le format '/images/file-name.extension'"
  }

  return null
}

const submitProduct = async () => {
  message.value = null
  error.value = null

  const validateError = validateProduct()

  if (validateError) {
    error.value = validateError
    return
  }

  try {
    await postProduct(product.value)
    message.value = 'Produit ajouté au shop !'

    //reset form
    product.value = {
      name: '',
      price: 0,
      imageUrl: '',
    }
  } catch (err: any) {
    if (axios.isAxiosError(err)) {
      if (err.response?.data?.errors) {
        const backendErrors = err.response.data.errors
        error.value = Object.values(backendErrors).flat().join(', ')
      } else {
        error.value = 'Erreur lors de la création du compte'
      }
    } else {
      error.value = 'Erreur inconnue'
    }
  }
}
</script>

<template>
  <div class="container">
    <h1>Ajouter un produit au shop</h1>

    <form @submit.prevent="submitProduct" class="form">
      <input
        data-testid="product-name"
        v-model="product.name"
        type="text"
        placeholder="Nom du produit"
      />
      <input
        data-testid="product-price"
        v-model.number="product.price"
        type="number"
        step="0.01"
        placeholder="Prix du produit"
        required
      />
      <input
        data-testid="product-image"
        v-model="product.imageUrl"
        type="text"
        placeholder="URL de l'image"
      />
      <button type="submit">Ajouter</button>
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
