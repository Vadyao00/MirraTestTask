import { useState, useEffect } from 'react'
import PropTypes from 'prop-types'

const ClientForm = ({ client, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    balanceT: 0
  })

  useEffect(() => {
    if (client) {
      setFormData({
        name: client.name,
        email: client.email,
        balanceT: client.balanceT
      })
    }
  }, [client])

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: name === 'balanceT' ? parseFloat(value) : value
    }))
  }

  const handleSubmit = (e) => {
    e.preventDefault()
    onSubmit(formData)
  }

  return (
    <form onSubmit={handleSubmit} className="client-form">
      <div className="form-group">
        <label htmlFor="name">Имя:</label>
        <input
          type="text"
          id="name"
          name="name"
          value={formData.name}
          onChange={handleChange}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="email">Email:</label>
        <input
          type="email"
          id="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="balanceT">Баланс (T):</label>
        <input
          type="number"
          id="balanceT"
          name="balanceT"
          value={formData.balanceT}
          onChange={handleChange}
          step="0.01"
          required
        />
      </div>

      <div className="form-actions">
        <button type="submit">
          {client ? 'Сохранить' : 'Создать'}
        </button>
        <button type="button" onClick={onCancel} className="cancel-btn">
          Отмена
        </button>
      </div>
    </form>
  )
}

ClientForm.propTypes = {
  client: PropTypes.shape({
    name: PropTypes.string,
    email: PropTypes.string,
    balanceT: PropTypes.number
  }),
  onSubmit: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired
}

export default ClientForm 