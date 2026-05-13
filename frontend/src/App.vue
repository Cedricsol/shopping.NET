<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useAuthStore } from './stores/authStore'
import router from './router'

const authStore = useAuthStore()

const logout = () => {
  authStore.logout()

  router.push('/login')
}
</script>

<template>
  <header>
    <RouterLink class="logo-home" to="/">
      <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />
    </RouterLink>

    <div class="wrapper">
      <nav>
        <RouterLink to="/">Boutique</RouterLink>
        <RouterLink v-if="authStore.isAdmin" to="/addProduct"
          >Ajouter un produit à la boutique</RouterLink
        >
        <RouterLink v-if="!authStore.isAuthenticated" to="/login" data-testid="add-product-link"
          >Connectez-vous</RouterLink
        >
        <button v-else @click="logout" class="logout-btn">Déconnexion</button>
      </nav>
    </div>
  </header>

  <RouterView />
</template>

<style scoped>
header {
  background: linear-gradient(90deg, #ff4081, #7c4dff);
  padding: 20px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 5);
}

.logo-home {
  width: 50%;
  display: flex;
  align-items: center;
}

.logo {
  display: block;
  margin: 0 auto;
  filter: drop-shadow(0 0 10px rgba(255, 255, 255, 0.3));
}

.wrapper {
  text-align: center;
  margin-top: 10px;
}

nav {
  margin-top: 10px;
}

nav a {
  margin: 0 10px;
  padding: 8px 15px;
  border-radius: 20px;
  background: rgba(255, 255, 255, 0.1);
  color: white;
  transition: 0.2s;
}

nav a:hover {
  background: var(--color-accent);
  color: black;
}

nav a.router-link-exact-active {
  background: white;
  color: black;
  font-weight: bold;
}

.logout-btn {
  margin: 0 10px;
  padding: 8px 15px;
  border-radius: 20px;
  border: none;
  background: rgba(255, 255, 255, 0.1);
  color: white;
  cursor: pointer;
  transition: 0.2s;
  font-size: 1rem;
}

.logout-btn:hover {
  background: var(--color-accent);
  color: black;
}

@media (min-width: 768px) {
  header {
    display: flex;
    align-items: center;
    justify-content: space-between;
  }

  .wrapper {
    text-align: right;
  }
}
</style>
