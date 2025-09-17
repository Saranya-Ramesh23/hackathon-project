@echo off
REM ==================================================
REM  Generate Certificates for mTLS (Server + Client)
REM  Local development ready for ASP.NET Core
REM ==================================================

REM Create working directory
set CERT_DIR=%cd%\mtls-certs
if not exist "%CERT_DIR%" mkdir "%CERT_DIR%"
cd "%CERT_DIR%"

REM =====================
echo === Generating CA Key and Certificate ===
openssl genrsa -out ca.key 4096
openssl req -x509 -new -nodes -key ca.key -sha256 -days 3650 ^
  -out ca.crt -subj "/C=US/ST=California/L=San Francisco/O=MyOrg/OU=CA/CN=MyRootCA"

echo === Exporting CA PFX (for optional trust) ===
openssl pkcs12 -export -out ca.pfx -inkey ca.key -in ca.crt -certfile ca.crt -password pass:changeme

REM =====================
echo === Generating Server Key and CSR ===
openssl genrsa -out server.key 2048
openssl req -new -key server.key -out server.csr ^
  -subj "/C=US/ST=California/L=San Francisco/O=MyOrg/OU=Server/CN=localhost"

REM =====================
echo === Creating SAN config for server ===
echo authorityKeyIdentifier=keyid,issuer>server.ext
echo basicConstraints=CA:FALSE>>server.ext
echo keyUsage = digitalSignature, keyEncipherment>>server.ext
echo extendedKeyUsage = serverAuth>>server.ext
echo subjectAltName = @alt_names>>server.ext
echo [alt_names]>>server.ext
echo DNS.1 = localhost>>server.ext
echo IP.1 = 127.0.0.1>>server.ext

REM =====================
echo === Signing Server Certificate with SAN ===
openssl x509 -req -in server.csr -CA ca.crt -CAkey ca.key -CAcreateserial ^
  -out server.crt -days 365 -sha256 -extfile server.ext

echo === Exporting Server PFX (for Kestrel) ===
openssl pkcs12 -export -out server.pfx -inkey server.key -in server.crt -certfile ca.crt -password pass:changeme

REM =====================
echo === Generating Client Key and Certificate ===
openssl genrsa -out client.key 2048
openssl req -new -key client.key -out client.csr ^
  -subj "/C=US/ST=California/L=San Francisco/O=MyOrg/OU=Client/CN=client.myorg.com"
openssl x509 -req -in client.csr -CA ca.crt -CAkey ca.key -CAcreateserial ^
  -out client.crt -days 365 -sha256

echo === Exporting Client PFX (for client usage) ===
openssl pkcs12 -export -out client.pfx -inkey client.key -in client.crt -certfile ca.crt -password pass:changeme

REM =====================
echo === Verifying Certificates ===
openssl verify -CAfile ca.crt server.crt
openssl verify -CAfile ca.crt client.crt

echo.
echo ==================================================
echo   Certificates generated in: %CERT_DIR%
echo   Files created:
echo     ca.crt / ca.key / ca.pfx
echo     server.crt / server.key / server.pfx
echo     client.crt / client.key / client.pfx
echo ==================================================
pause
