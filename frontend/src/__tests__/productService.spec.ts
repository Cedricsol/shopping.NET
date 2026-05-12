import { describe, it, expect, vi, beforeEach } from 'vitest'
import { getProducts, postProduct, type Product } from '@/services/productService'
import api from '@/api/api'

vi.mock('@/api/api', () => ({
  default: {
    get: vi.fn(),
    post: vi.fn(),
  },
}))

describe('productService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('Call GET /api/products', async () => {
    const mockData: Product[] = [{ name: 'Produit 1', price: 10, imageUrl: 'img1.jpg' }]

    vi.mocked(api.get).mockResolvedValueOnce({ data: mockData })

    const response = await getProducts()

    expect(api.get).toHaveBeenCalledWith('/products')
    expect(response.data).toEqual(mockData)
  })

  it('Call POST /api/products', async () => {
    const product: Product = {
      name: 'Produit test',
      price: 42,
      imageUrl: 'img.jpg',
    }

    vi.mocked(api.post).mockResolvedValueOnce({ data: {} })

    await postProduct(product)

    expect(api.post).toHaveBeenCalledWith('/products', product)
  })
})
