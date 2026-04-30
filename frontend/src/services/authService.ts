import axios from 'axios'
import api from '../api/api'

export enum UserRole {
  User,
  Admin,
}

export interface RegisterDto {
  email: string
  username: string
  password: string
}

export interface LoginDto {
  email: string
  password: string
}

export interface AuthResponse {
  token: string
  email: string
  username: string
  role: UserRole
}

export function register(register: RegisterDto) {
  return api.post<AuthResponse>(`/auth/register`, register)
}

export function login(login: LoginDto) {
  return axios.post<AuthResponse>(`/auth/login`, login)
}
