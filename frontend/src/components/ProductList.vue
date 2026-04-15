<script setup lang="ts">
import { getProducts, type Product } from '@/services/productService'
import { onMounted, ref } from 'vue'

const products = ref<Product[]>([])
const error = ref<string | null>(null)

const fetchProducts = async () => {
  try {
    const response = await getProducts()
    products.value = response.data
  } catch (err) {
    error.value = 'Erreur lors du chargement'
  }
}

onMounted(fetchProducts)
</script>

<style scoped>
.container {
  padding: 20px;
}

.grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}

.card {
  border: 1px solid #ddd;
  padding: 10px;
}

.card img {
  width: 100%;
  height: 200px;
  object-fit: cover;
}
</style>

<template>
  <div class="container">
    <h1>Produits</h1>

    <p v-if="products.length === 0">Chargement...</p>
    <p v-if="error">{{ error }}</p>

    <div class="grid">
      <div v-for="product in products" :key="product.name" class="card">
        <img :src="product.imageUrl" alt="" />
        <h2>{{ product.name }}</h2>
        <p>{{ product.price }} €</p>
      </div>
    </div>
  </div>
</template>
