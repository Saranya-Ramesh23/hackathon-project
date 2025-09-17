# High Level Design (HLD)

## Context
Device provisioning platform with three components: Registry, Issuer, Frontend.

## Components
- Registry Service: Stores device metadata and registration status.
- Issuer Service: Validates device registration and issues X.509 device certificates signed by a dev CA.
- Frontend: Angular UI for registering devices and requesting/downloading certificates.

## Sequence (simplified)
1. User registers device via frontend -> POST /registry/devices
2. User requests issuance -> POST /issuer/enroll
3. Issuer validates device with Registry, issues cert, stores artifact
4. Frontend downloads cert -> GET /issuer/certs/{deviceId}
