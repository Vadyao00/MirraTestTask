import axios from 'axios'

const API_BASE_URL = 'http://localhost:5000'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response.data,
  async (error) => {
    const originalRequest = error.config

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true
      
      try {
        const refreshToken = localStorage.getItem('refreshToken')
        const response = await api.post('/auth/refresh', { 
          token: localStorage.getItem('token'),
          refreshToken 
        })
        
        localStorage.setItem('token', response.token)
        localStorage.setItem('refreshToken', response.refreshToken)
        
        return api(originalRequest)
      } catch (err) {
        localStorage.removeItem('token')
        localStorage.removeItem('refreshToken')
        window.location.href = '/login'
      }
    }
    
    return Promise.reject(error)
  }
)

export const authService = {
  async login(email, password) {
    const response = await api.post('/auth/login', { email, password })
    localStorage.setItem('token', response.token)
    localStorage.setItem('refreshToken', response.refreshToken)
    return response
  },

  async refreshToken(token, refreshToken) {
    const response = await api.post('/auth/refresh', { token, refreshToken })
    return response
  }
}

export const clientService = {
  async getClients() {
    const response = await api.get('/clients')
    return response
  },

  async createClient(clientData) {
    const response = await api.post('/clients', clientData)
    return response
  },

  async updateClient(id, clientData) {
    const response = await api.put(`/clients/${id}`, clientData)
    return response
  },

  async deleteClient(id) {
    const response = await api.delete(`/clients/${id}`)
    return response
  }
}

export const paymentService = {
  async getPayments(take = 5) {
    const response = await api.get(`/payments?take=${take}`)
    return response
  },

  async getClientPayments(clientId) {
    const response = await api.get(`payments/client/${clientId}`)
    return response
  }
}

export const rateService = {
  async getRate() {
    const response = await api.get('/rate')
    return response
  },

  async updateRate(rate) {
    const response = await api.post('/rate', { rate })
    return response
  }
}

export default api