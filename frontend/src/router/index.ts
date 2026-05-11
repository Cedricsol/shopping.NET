import { createRouter, createWebHistory } from 'vue-router'
import ProductList from '@/components/ProductList.vue'
import ProductForm from '@/components/ProductForm.vue'
import Login from '@/components/Login.vue'
import Register from '@/components/Register.vue'
import { useAuthStore } from '@/stores/authStore'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Shop',
      component: ProductList,
    },
    {
      path: '/addProduct',
      name: 'addProduct',
      component: ProductForm,
      meta: {
        requiresAdmin: true,
      },
    },
    {
      path: '/login',
      name: 'login',
      component: Login,
    },
    {
      path: '/register',
      name: 'register',
      component: Register,
    },
  ],
})

router.beforeEach((to) => {
  const authStore = useAuthStore()

  if (to.meta.requiresAdmin && !authStore.isAdmin) {
    return '/login'
  }

  return true
})

export default router
