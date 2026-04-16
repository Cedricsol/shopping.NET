import { mount } from '@vue/test-utils'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import ProductForm from '@/components/ProductForm.vue'
import * as productService from '@/services/productService'
import type { InternalAxiosRequestConfig } from 'axios'

vi.mock('@/services/productService', () => ({
  postProduct: vi.fn(),
}))

describe('ProductForm.vue', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('Display form correctly', () => {
    const wrapper = mount(ProductForm)

    expect(wrapper.find('h1').text()).toBe('Ajouter un produit au shop')
    expect(wrapper.find('input[placeholder="Nom du produit"]').exists()).toBe(true)
    expect(wrapper.find('input[placeholder="Prix du produit"]').exists()).toBe(true)
    expect(wrapper.find('input[placeholder="URL de l\'image"]').exists()).toBe(true)
    expect(wrapper.find('button').text()).toBe('Ajouter')
  })

  it('Submit form successfully', async () => {
    const wrapper = mount(ProductForm)

    vi.spyOn(productService, 'postProduct').mockResolvedValueOnce({
      data: {},
      status: 200,
      statusText: 'OK',
      headers: {},
      config: {
        headers: {},
      } as InternalAxiosRequestConfig,
    })

    // Fill form
    await wrapper.find('input[placeholder="Nom du produit"]').setValue('Produit test')
    await wrapper.find('input[placeholder="Prix du produit"]').setValue('10')
    await wrapper.find('input[placeholder="URL de l\'image"]').setValue('http://image.com')

    // Submit form
    await wrapper.find('form').trigger('submit.prevent')

    expect(productService.postProduct).toHaveBeenCalledWith({
      name: 'Produit test',
      price: 10,
      imageUrl: 'http://image.com',
    })

    // Test success message
    expect(wrapper.text()).toContain('Produit ajouté au shop !')

    // Test for reset
    const inputs = wrapper.findAll('input')
    expect((inputs[0]?.element as HTMLInputElement).value).toBe('')
    expect((inputs[1]?.element as HTMLInputElement).value).toBe('0')
    expect((inputs[2]?.element as HTMLInputElement).value).toBe('')
  })

  it('Display error if submit fails', async () => {
    const wrapper = mount(ProductForm)

    vi.spyOn(productService, 'postProduct').mockRejectedValueOnce(new Error('API error'))

    await wrapper.find('form').trigger('submit.prevent')

    expect(wrapper.text()).toContain("Erreur lors de l'ajout du produit")
  })
})
