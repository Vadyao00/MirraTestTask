# Admin Dashboard (Minimal Slice)

## Установка и запуск проекта

### Клонирование репозитория

```bash
git clone https://github.com/Vadyao00/MirraTestTask.git

cd MirraTestTask
```

### Локальный запуск

1. **Backend (API)**
   ```bash
   cd backend/MirraApi/Presentation/Mirra.API
   dotnet run
   ```
   API будет доступно по адресу: `http://localhost:5000`

2. **Frontend**
   ```bash
   cd frontend/MirraFront
   npm install
   npm run dev
   ```
   Фронтенд будет доступен по адресу: `http://localhost:5173`

### Запуск через Docker Compose

1. Убедитесь, что у вас установлен Docker и Docker Compose
2. В корневой директории проекта выполните:
   ```bash
   docker-compose up -d
   ```
   Это запустит все необходимые сервисы:
   - Frontend: `http://localhost:5173`
   - Backend API: `http://localhost:5000`
   - PostgreSQL Database

## Continuous Integration

Проект настроен с использованием GitHub Actions для непрерывной интеграции. При каждом push в main ветку или создании pull request автоматически выполняются:

- Сборка backend и frontend
- Запуск всех тестов
- Проверка успешности сборки

## Данные для входа

```
Email: admin@mirra.dev
Password: admin123
```

## API Endpoints

### Аутентификация
```bash
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@mirra.dev", "password": "admin123"}'
```

### Клиенты
```bash
curl -X GET http://localhost:5000/clients \
  -H "Authorization: Bearer {your_token}"
```

### Платежи
```bash
curl -X GET http://localhost:5000/payments?take=5 \
  -H "Authorization: Bearer {your_token}"
```

### Курс токенов
```bash
curl -X GET http://localhost:5000/rate \
  -H "Authorization: Bearer {your_token}"

curl -X POST http://localhost:5000/rate \
  -H "Authorization: Bearer {your_token}" \
  -H "Content-Type: application/json" \
  -d '{"rate": 12.5}'
```

## Примечания

- При запуске все необходимые миграции и начальные данные применяются автоматически
- В локальном режиме используется SQLite (InMemory), в Docker - PostgreSQL
- Для работы с API через Postman или другой инструмент необходимо использовать токен, полученный после аутентификации 