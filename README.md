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
export Jwt__Key="replace-with-a-long-random-demo-key"
dotnet run
```

## Quick test

1. Get token:

```bash
TOKEN=$(curl -s -X POST http://localhost:5157/login \
  -H "Content-Type: application/json" \
  -d '{"username":"demo","password":"password123"}' | \
  python -c 'import sys,json; print(json.load(sys.stdin)["accessToken"])')
```

2. Call protected endpoint:

```bash
curl -s http://localhost:5157/secure --oauth2-bearer "$TOKEN"
```

Optionally set a stable signing key (instead of the generated in-memory key) by setting environment variable `Jwt__Key` before running the app.