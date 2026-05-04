import { mount } from '@vue/test-utils'
import { vi, describe, it, expect, beforeEach } from 'vitest'
import ProductList from '@/components/ProductList.vue'
import * as productService from '@/services/productService'
import type { AxiosResponse, InternalAxiosRequestConfig } from 'axios'
import { nextTick } from 'vue'

vi.mock('@/services/productService', () => ({
  getProducts: vi.fn(),
}))

describe('ProductList.vue', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('Display "Chargement" at the beginning', () => {
    vi.spyOn(productService, 'getProducts').mockResolvedValueOnce({
      data: [],
      status: 200,
      statusText: 'OK',
      headers: {},
      config: {
        headers: {},
      } as InternalAxiosRequestConfig,
    })

    const wrapper = mount(ProductList)

    expect(wrapper.text()).toContain('Chargement...')
  })

  it('Display products after loading', async () => {
    const mockProducts = [
      { name: 'Produit 1', price: 10, imageUrl: 'img1.jpg' },
      { name: 'Produit 2', price: 30, imageUrl: 'img2.jpg' },
    ]

    vi.spyOn(productService, 'getProducts').mockResolvedValueOnce({
      data: mockProducts,
      status: 200,
      statusText: 'OK',
      headers: {},
      config: {
        headers: {},
      } as InternalAxiosRequestConfig,
    })

    const wrapper = mount(ProductList)

    await nextTick() // onMounted
    await nextTick() // update DOM

    const cards = wrapper.findAll('.card')
    expect(cards.length).toBe(2)

    expect(wrapper.text()).toContain('Produit 1')
    expect(wrapper.text()).toContain('Produit 2')
    expect(wrapper.text()).toContain('10 €')
    expect(wrapper.text()).toContain('30 €')
  })

  it('Display error if API call fails', async () => {
    vi.spyOn(productService, 'getProducts').mockRejectedValueOnce(new Error('API error'))

    const wrapper = mount(ProductList)

    await nextTick() // onMounted
    await nextTick() // update DOM

    expect(wrapper.text()).toContain('Erreur lors du chargement')
  })
})
