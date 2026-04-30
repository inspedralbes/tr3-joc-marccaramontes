const WebSocket = require('ws');

async function testIdentity() {
    console.log('--- Testing Identity Injection ---');
    const ws1 = new WebSocket('ws://localhost:3000/ws');
    const ws2 = new WebSocket('ws://localhost:3000/ws');

    let client1Joined = false;
    let client2Joined = false;

    ws1.on('open', () => {
        ws1.send(JSON.stringify({ type: 'JOIN_ROOM', payload: JSON.stringify({ roomId: 'ID_TEST', playerName: 'Player1' }) }));
    });

    ws2.on('open', () => {
        ws2.send(JSON.stringify({ type: 'JOIN_ROOM', payload: JSON.stringify({ roomId: 'ID_TEST', playerName: 'Player2' }) }));
    });

    ws1.on('message', (data) => {
        const msg = JSON.parse(data);
        if (msg.type === 'ROOM_JOINED_CONFIRMED') client1Joined = true;
        if (msg.type === 'PLAYER_JOINED') console.log('[Client 1] Player joined:', JSON.parse(msg.payload).playerName);
        
        if (msg.type === 'MOVE') {
            console.log('[Client 1] Received MOVE from:', msg.playerId, 'payload:', msg.payload);
            if (msg.playerId === 'Player2') {
                console.log('Identity Injection Test SUCCESSFUL!');
                process.exit(0);
            }
        }
    });

    ws2.on('message', (data) => {
        const msg = JSON.parse(data);
        if (msg.type === 'ROOM_JOINED_CONFIRMED') {
            client2Joined = true;
            // Send move after joining
            setTimeout(() => {
                console.log('[Client 2] Sending MOVE...');
                ws2.send(JSON.stringify({ type: 'MOVE', payload: JSON.stringify({ x: 10, y: 20 }) }));
            }, 500);
        }
    });
}

testIdentity();
