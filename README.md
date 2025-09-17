# Device Provisioning Platform

This repository contains a minimal, production‑minded scaffold for a device identity and provisioning platform.  
It includes three components:

- **registry-svc** — .NET microservice for device registration
- **issuer-svc** — .NET microservice for certificate issuance
- **frontend** — Angular application for user interaction

---

## Prerequisites

- Node.js (>= 16) and Angular CLI for local frontend development
- .NET 7 SDK for backend services
- Docker / Docker Compose for containerized runs
- Kubernetes (optional) for cluster deployment
- A development CA certificate (PFX) for issuer service

---

## Running Locally (without Docker)

### Backend
1. Navigate to each backend service folder:
   ```bash
   cd registry-svc
   dotnet run
   ```
   ```bash
   cd issuer-svc
   dotnet run
   ```
   By default they listen on ports 5000/5001.

2. Configure environment variables for certificates:
   -  Configure a pfx and password in appsettings.json for the Issuer service
   - `REGISTRY_URL` for the Issuer service to reach the Registry service

### Frontend
1. Navigate to the `frontend` folder:
   ```bash
   cd frontend
   npm install
   npm start
   ```
   The app runs on [http://localhost:4200](http://localhost:4200).

2. Set API base URL by injecting `API_BASE_URL` at runtime.

---

## Running with Docker Compose

1. Create a `docker-compose.yml` (to be added) that runs the three containers and connects them on a shared network.

2. Mount secrets (certificates, passwords) as environment variables or volumes — do **not** hardcode them in code or images.

3. Example build and run:
   ```bash
   docker-compose build
   docker-compose up
   ```

---

## Next Steps

- Add **health checks** endpoints to both backend services for readiness/liveness probes.
- Implement **observability**: structured logs with correlation IDs, optional metrics.
- Strengthen **security**: enforce mTLS properly (validate client certs against your CA chain).
- Replace in‑memory stores with persistent storage if needed.
- Add **Kubernetes manifests** (`/deploy`) for Deployments, Services, Ingress, HPA, and Secrets.

---

## Acceptance Flow

1. Register a device in the Registry via frontend (or REST API).
2. Request certificate issuance for the device in the Issuer service.
3. Download the resulting certificate artifact from the frontend.

---

## Repository Structure

```
/registry-svc   (microservice)
/issuer-svc     (microservice)
/frontend       (Angular app)
/scripts        (OpenSSL Script)
README.md       (this file)
```
