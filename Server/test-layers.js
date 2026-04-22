const axios = require('axios');

async function testLayers() {
    console.log('--- Testing API Layers ---');
    const GATEWAY_URL = 'http://localhost:3000/api';

    try {
        // 1. Create Room
        console.log('Testing Room Creation...');
        const createRes = await axios.post(`${GATEWAY_URL}/rooms/create`, { playerName: 'Architect' });
        console.log('Create Room Response:', createRes.data);
        const roomId = createRes.data.roomId;

        if (!roomId) throw new Error('Room ID not received');

        // 2. Join Room
        console.log('Testing Room Joining...');
        const joinRes = await axios.post(`${GATEWAY_URL}/rooms/join`, { roomId, playerName: 'Builder' });
        console.log('Join Room Response:', joinRes.data);

        // 3. Save Results
        console.log('Testing Result Saving...');
        const resultRes = await axios.post(`${GATEWAY_URL}/results`, { 
            roomId, 
            playerName: 'Architect', 
            survivalTime: 42.5 
        });
        console.log('Save Results Response:', resultRes.data);

        console.log('Layered Architecture Test SUCCESSFUL');
    } catch (err) {
        console.error('Layered Architecture Test FAILED:', err.response ? err.response.data : err.message);
    }
}

testLayers();
