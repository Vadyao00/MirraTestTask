import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { clientService, rateService } from '../services/api'
import ClientForm from '../components/ClientForm'
import PaymentHistory from '../components/PaymentHistory'

const Dashboard = () => {
  const [clients, setClients] = useState([])
  const [rate, setRate] = useState(10)
  const [newRate, setNewRate] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [selectedClient, setSelectedClient] = useState(null)
  const [showClientForm, setShowClientForm] = useState(false)
  const [showPayments, setShowPayments] = useState(false)
  const navigate = useNavigate()

  const handleLogout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
    navigate('/login')
  }

  const loadData = async () => {
    try {
      setLoading(true)
      const [clientsData, rateData] = await Promise.all([
        clientService.getClients(),
        rateService.getRate()
      ])
      
      setClients(clientsData)
      setRate(rateData.rate)
    } catch (err) {
      setError('Ошибка загрузки данных')
    } finally {
      setLoading(false)
    }
  }

  const handleRateUpdate = async (e) => {
    e.preventDefault()
    if (!newRate || newRate <= 0) return

    try {
      const response = await rateService.updateRate(parseFloat(newRate))
      setRate(response.rate)
      setNewRate('')
    } catch (err) {
      setError('Ошибка обновления курса')
    }
  }

  const handleClientSubmit = async (clientData) => {
    try {
      if (selectedClient) {
        await clientService.updateClient(selectedClient.id, clientData)
      } else {
        await clientService.createClient(clientData)
      }
      await loadData()
      setShowClientForm(false)
      setSelectedClient(null)
    } catch (err) {
      setError('Ошибка при сохранении клиента')
    }
  }

  const handleDeleteClient = async (id) => {
    try {
      await clientService.deleteClient(id)
      await loadData()
    } catch (err) {
      setError('Ошибка при удалении клиента')
    }
  }

  const handleEditClient = (client) => {
    setSelectedClient(client)
    setShowClientForm(true)
  }

  useEffect(() => {
    loadData()
  }, [])

  if (loading) {
    return <div className="loading">Загрузка...</div>
  }

  return (
    <div className="dashboard">
      <header className="dashboard-header">
        <h1>Панель управления</h1>
        <button onClick={handleLogout} className="logout-btn">
          Выйти
        </button>
      </header>

      {error && (
        <div className="error-banner">
          {error}
          <button onClick={() => setError('')}>×</button>
        </div>
      )}

      <div className="dashboard-content">
        <section className="clients-section">
          <div className="section-header">
            <h2>Клиенты</h2>
            <button 
              onClick={() => {
                setSelectedClient(null)
                setShowClientForm(true)
              }}
              className="add-btn"
            >
              Добавить клиента
            </button>
          </div>

          {showClientForm ? (
            <ClientForm
              client={selectedClient}
              onSubmit={handleClientSubmit}
              onCancel={() => {
                setShowClientForm(false)
                setSelectedClient(null)
              }}
            />
          ) : (
            <div className="table-container">
              <table className="clients-table">
                <thead>
                  <tr>
                    <th>Имя</th>
                    <th>Email</th>
                    <th>Баланс (T)</th>
                    <th>Действия</th>
                  </tr>
                </thead>
                <tbody>
                  {clients.map(client => (
                    <tr key={client.id}>
                      <td>{client.name}</td>
                      <td>{client.email}</td>
                      <td className="balance">{client.balanceT.toFixed(2)}</td>
                      <td>
                        <div className="action-buttons">
                          <button
                            onClick={() => handleEditClient(client)}
                            className="edit-btn"
                          >
                            ✎
                          </button>
                          <button
                            onClick={() => handleDeleteClient(client.id)}
                            className="delete-btn"
                          >
                            ×
                          </button>
                          <button
                            onClick={() => {
                              setSelectedClient(client)
                              setShowPayments(true)
                            }}
                            className="history-btn"
                          >
                            История
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </section>

        <section className="rate-section">
          <h2>Курс токенов</h2>
          <div className="rate-container">
            <div className="current-rate">
              <span className="rate-label">Текущий курс:</span>
              <span className="rate-value">{rate}</span>
              <span className="rate-unit">BYN/T</span>
            </div>
            
            <form onSubmit={handleRateUpdate} className="rate-form">
              <div className="form-group">
                <label htmlFor="newRate">Новый курс:</label>
                <input
                  type="number"
                  id="newRate"
                  step="0.01"
                  min="0.01"
                  max="1000"
                  value={newRate}
                  onChange={(e) => setNewRate(e.target.value)}
                  placeholder="Введите новый курс"
                />
              </div>
              <button type="submit" disabled={!newRate}>
                Обновить курс
              </button>
            </form>
          </div>
        </section>

        {showPayments && selectedClient ? (
          <section className="payments-section">
            <div className="section-header">
              <h2>История платежей - {selectedClient.name}</h2>
              <button 
                onClick={() => {
                  setShowPayments(false)
                  setSelectedClient(null)
                }}
                className="close-btn"
              >
                Закрыть
              </button>
            </div>
            <PaymentHistory clientId={selectedClient.id} />
          </section>
        ) : (
          <section className="payments-section">
            <h2>Последние платежи</h2>
            <PaymentHistory />
          </section>
        )}
      </div>
    </div>
  )
}

export default Dashboard