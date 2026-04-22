const axios = require('axios');
const WebSocket = require('ws');

async function testIntegration() {
    console.log('--- Testing Integration ---');

    try {
        // 1. Test Gateway -> API Service
        console.log('Testing API routing...');
        const apiRes = await axios.get('http://localhost:3000/api/health');
        console.log('API Response:', apiRes.data);

        // 2. Test Gateway -> Game Service (WS)
        console.log('Testing WS routing...');
        const ws = new WebSocket('ws://localhost:3000/ws');
        
        ws.on('open', () => {
            console.log('WS Connection established via Gateway');
            ws.send(JSON.stringify({ 
                type: 'JOIN_ROOM', 
                payload: { roomId: 'TEST_ROOM', playerName: 'Tester' } 
            }));
        });

        ws.on('message', (data) => {
            const msg = JSON.parse(data);
            console.log('WS Received:', msg);
            if (msg.type === 'ROOM_JOINED_CONFIRMED') {
                console.log('Integration Test SUCCESSFUL');
                ws.close();
            }
        });

        ws.on('error', (err) => {
            console.error('WS Error:', err.message);
        });

    } catch (err) {
        console.error('Integration Test FAILED:', err.message);
    }
}

testIntegration();
