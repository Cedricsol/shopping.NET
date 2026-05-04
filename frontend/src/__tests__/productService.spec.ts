import { describe, it, expect, vi, beforeEach } from 'vitest'
import axios from 'axios'
import { getProducts, postProduct, type Product } from '@/services/productService'

vi.mock('axios')

describe('productService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('Call GET /api/products', async () => {
    const mockData: Product[] = [{ name: 'Produit 1', price: 10, imageUrl: 'img1.jpg' }]

    ;(axios.get as any).mockResolvedValueOnce({ data: mockData })

    const response = await getProducts()

    expect(axios.get).toHaveBeenCalledWith('http://localhost:5039/api/products')
    expect(response.data).toEqual(mockData)
  })

  it('Call POST /api/products', async () => {
    const product: Product = {
      name: 'Produit test',
      price: 42,
      imageUrl: 'img.jpg',
    }

    ;(axios.post as any).mockResolvedValueOnce({ data: {} })

    await postProduct(product)

    expect(axios.post).toHaveBeenCalledWith('http://localhost:5039/api/products', product)
  })
})
