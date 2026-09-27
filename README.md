# JWTDemo

This project demonstrates JWT authentication using ASP.NET Core minimal APIs.

## Endpoints

- `POST /login` (anonymous): Returns a JWT token for demo credentials.
- `GET /secure` (authorized): Requires a valid bearer token.

## Demo credentials

- Username: `demo`
- Password: `password123`

## Run locally

```bash
dotnet restore
dotnet run
```

## Quick test

1. Get token:

```bash
curl -s -X POST http://localhost:5000/login \
  -H "Content-Type: application/json" \
  -d '{"username":"demo","password":"password123"}'
```

2. Call protected endpoint:

```bash
curl -s http://localhost:5000/secure \
  -H "Authorization: ******"
```

Use the `accessToken` returned from `/login` as the bearer token value.