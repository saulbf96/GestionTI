const { env } = require('process');
const https = require('https');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7159';

// Solo desarrollo. La autenticación de Windows (NTLM) necesita que todos los pasos del
// intercambio viajen por la MISMA conexión hacia la API. Sin este agente, el proxy cierra
// la conexión en cada petición y la API responde 401 siempre.
const windowsAuthAgent = new https.Agent({
  keepAlive: true,
  maxSockets: 1,
  keepAliveMsecs: 1500,
  rejectUnauthorized: false, // certificado de desarrollo de localhost
});

const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target,
    secure: false,
    agent: windowsAuthAgent,
  }
]

module.exports = PROXY_CONFIG;
