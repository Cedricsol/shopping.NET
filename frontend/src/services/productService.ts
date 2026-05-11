import api from '@/api/api'

export interface Product {
  name: string
  price: number
  imageUrl: string
}

export function getProducts() {
  return api.get<Product[]>('http://localhost:5039/api/products')
}

export function postProduct(product: Product) {
  return api.post('http://localhost:5039/api/products', product)
}
