# Admin Dashboard (Minimal Slice)

Тестовый проект админ-панели с функционалом управления клиентами, платежами и курсом токенов.

## Технологический стек

### Backend
- ASP.NET Core 8 (Minimal API)
- Entity Framework Core
- SQLite (для локальной разработки)
- PostgreSQL (для Docker-окружения)
- JWT Authentication

### Frontend
- React
- Vite
- TypeScript
- Axios

## Функциональность

- Аутентификация пользователей (JWT)
- Управление клиентами (просмотр списка)
- Управление курсом токенов (просмотр и изменение)
- Просмотр последних платежей
- Docker-окружение для развертывания

## Запуск проекта

Проект можно запустить двумя способами: локально или через Docker Compose.

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

## Данные для входа

```
Email: admin@mirra.dev
Password: admin123
```

## API Endpoints

### Аутентификация
```bash
# Логин
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@mirra.dev", "password": "admin123"}'
```

### Клиенты
```bash
# Получение списка клиентов
curl -X GET http://localhost:5000/clients \
  -H "Authorization: Bearer {your_token}"
```

### Платежи
```bash
# Получение последних 5 платежей
curl -X GET http://localhost:5000/payments?take=5 \
  -H "Authorization: Bearer {your_token}"
```

### Курс токенов
```bash
# Получение текущего курса
curl -X GET http://localhost:5000/rate \
  -H "Authorization: Bearer {your_token}"

# Обновление курса
curl -X POST http://localhost:5000/rate \
  -H "Authorization: Bearer {your_token}" \
  -H "Content-Type: application/json" \
  -d '{"rate": 12.5}'
```

## Дополнительные возможности

- Полноценная JWT-аутентификация с refresh-токенами
- Docker-окружение с PostgreSQL
- Автоматические миграции при запуске
- Начальные данные (seed data) для тестирования

## Структура проекта

```
├── backend/
│   └── MirraApi/
│       ├── Core/
│       │   ├── Contracts/
│       │   ├── MirraApi.Application/
│       │   └── MirraApi.Domain/
│       ├── Infrastructure/
│       │   └── MirraApi.Persistence/
│       └── Presentation/
│           └── Mirra.API/
├── frontend/
│   └── MirraFront/
└── docker-compose.yml
```

## Примечания

- При запуске через Docker все необходимые миграции и начальные данные применяются автоматически
- В локальном режиме используется SQLite, в Docker - PostgreSQL
- Для работы с API через Postman или другой инструмент необходимо использовать токен, полученный после аутентификации 