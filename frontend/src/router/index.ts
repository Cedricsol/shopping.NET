import { createRouter, createWebHistory } from 'vue-router'
import ProductList from '@/components/ProductList.vue'
import ProductForm from '@/components/ProductForm.vue'

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
  ],
})

export default router
