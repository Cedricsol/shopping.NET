import axios from 'axios'

export interface Product {
  name: string
  price: number
  imageUrl: string
}

export function getProducts() {
  return axios.get<Product[]>('http://localhost:5039/api/products')
}

export function postProduct(product: Product) {
  return axios.post('http://localhost:5039/api/products', product)
}
