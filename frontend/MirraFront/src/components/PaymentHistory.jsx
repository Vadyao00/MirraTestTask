import { useState, useEffect } from 'react'
import PropTypes from 'prop-types'
import { paymentService } from '../services/api'

const PaymentHistory = ({ clientId }) => {
  const [payments, setPayments] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadPayments = async () => {
      try {
        setLoading(true)
        const data = clientId 
          ? await paymentService.getClientPayments(clientId)
          : await paymentService.getPayments(5)
        setPayments(data)
      } catch (err) {
        setError('Ошибка загрузки платежей')
      } finally {
        setLoading(false)
      }
    }

    loadPayments()
  }, [clientId])

  if (loading) {
    return <div className="loading">Загрузка платежей...</div>
  }

  if (error) {
    return <div className="error">{error}</div>
  }

  return (
    <div className="payment-history">
      <h3>{clientId ? 'История платежей клиента' : 'Последние платежи'}</h3>
      <div className="table-container">
        <table className="payments-table">
          <thead>
            <tr>
              <th>Дата</th>
              <th>Сумма (T)</th>
              <th>Описание</th>
              {!clientId && <th>Клиент</th>}
            </tr>
          </thead>
          <tbody>
            {payments.map(payment => (
              <tr key={payment.id}>
                <td>{new Date(payment.date).toLocaleDateString()}</td>
                <td className="amount">{payment.amount.toFixed(2)}</td>
                <td>{payment.description}</td>
                {!clientId && <td>{payment.clientName}</td>}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

PaymentHistory.propTypes = {
  clientId: PropTypes.number
}

export default PaymentHistory 