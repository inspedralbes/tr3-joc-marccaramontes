const { spawn } = require('child_process');
const path = require('path');

const services = [
    { name: 'Gateway', dir: 'gateway', port: 3000 },
    { name: 'API Service', dir: 'api-service', port: 3001 },
    { name: 'Game Service', dir: 'game-service', port: 3002 }
];

services.forEach(service => {
    console.log(`[Manager] Starting ${service.name}...`);
    const proc = spawn('node', ['index.js'], {
        cwd: path.join(__dirname, service.dir),
        stdio: 'inherit',
        env: { ...process.env, PORT: service.port }
    });

    proc.on('error', (err) => {
        console.error(`[Manager] Error in ${service.name}:`, err);
    });

    proc.on('exit', (code) => {
        console.log(`[Manager] ${service.name} exited with code ${code}`);
    });
});
