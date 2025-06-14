import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { authService } from '../services/api'

const Login = () => {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  const handleSubmit = async (e) => {
    e.preventDefault()
    
    if (loading) return
    
    setLoading(true)
    setError('')

    try {
      const response = await authService.login(email, password)
      if (response.token) {
        localStorage.setItem('token', response.token)
        navigate('/dashboard')
      } else {
        throw new Error('Токен не получен')
      }
    } catch (err) {
      setError('Неверные учетные данные')
      setTimeout(() => {
        setError('')
      }, 3000)
    }
    
    setLoading(false)
  }

  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Вход в систему</h1>
        <form onSubmit={handleSubmit} noValidate>
          <div className="form-group">
            <label htmlFor="email">Email:</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              placeholder="admin@mirra.dev"
              disabled={loading}
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="password">Пароль:</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              placeholder="admin123"
              disabled={loading}
            />
          </div>
          
          {error && (
            <div className="error">
              <span>{error}</span>
            </div>
          )}
          
          <button type="submit" disabled={loading}>
            {loading ? 'Вход...' : 'Войти'}
          </button>
        </form>
        
        <div className="demo-info">
          <p>Демо данные:</p>
          <p>Email: admin@mirra.dev</p>
          <p>Пароль: admin123</p>
        </div>
      </div>
    </div>
  )
}

export default Login