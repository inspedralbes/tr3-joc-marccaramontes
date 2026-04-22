const express = require('express');
const http = require('http');
const httpProxy = require('http-proxy');

const app = express();
const proxy = httpProxy.createProxyServer({});

const API_SERVICE_URL = 'http://localhost:3001';
const GAME_SERVICE_WS_URL = 'ws://localhost:3002';

// Health check
app.get('/health', (req, res) => {
    res.json({ status: 'Gateway is running' });
});

// Route HTTP /api requests to API Service
app.all('/api/*', (req, res) => {
    console.log(`[Gateway] Routing API request: ${req.url}`);
    proxy.web(req, res, { target: API_SERVICE_URL }, (err) => {
        console.error('[Gateway] API Proxy Error:', err.message);
        res.status(502).send('API Service Unavailable');
    });
});

const server = http.createServer(app);

// Route WebSocket /ws requests to Game Service
server.on('upgrade', (req, socket, head) => {
    if (req.url === '/ws') {
        console.log('[Gateway] Routing WS upgrade request');
        proxy.ws(req, socket, head, { target: GAME_SERVICE_WS_URL }, (err) => {
            console.error('[Gateway] WS Proxy Error:', err.message);
            socket.destroy();
        });
    } else {
        socket.destroy();
    }
});

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
    console.log(`[Gateway] running on http://localhost:${PORT}`);
});
