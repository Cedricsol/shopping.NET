import api from '@/api/api'

export interface Product {
  name: string
  price: number
  imageUrl: string
}

export function getProducts() {
  return api.get<Product[]>('/products')
}

export function postProduct(product: Product) {
  return api.post('/products', product)
}
