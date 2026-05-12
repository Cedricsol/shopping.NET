const TOKEN_KEY = 'auth_token'
const ROLE_KEY = 'auth_role'

export function setToken(token: string) {
  sessionStorage.setItem(TOKEN_KEY, token)
}

export function getToken(): string | null {
  return sessionStorage.getItem(TOKEN_KEY)
}

export function removeToken() {
  sessionStorage.removeItem(TOKEN_KEY)
}

export function setRole(role: string) {
  sessionStorage.setItem(ROLE_KEY, role)
}

export function getRole(): string | null {
  return sessionStorage.getItem(ROLE_KEY)
}

export function removeRole() {
  sessionStorage.removeItem(ROLE_KEY)
}
