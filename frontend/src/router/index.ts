import { createRouter, createWebHistory } from 'vue-router'
import ProductList from '@/components/ProductList.vue'
import ProductForm from '@/components/ProductForm.vue'
import Login from '@/components/Login.vue'
import Register from '@/components/Register.vue'

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

export default router
