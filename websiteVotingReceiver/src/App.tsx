import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import ServerAddress from './ServerAddress'
import { UserName } from './UserName'
import useWebSocket, { ReadyState } from 'react-use-websocket';
import VoteScreen from './VoteScreen'

function App() {
  const [count, setCount] = useState(0)
  const [serverAddress, setNewServerAddress] = useState('');
  const [username, setUsername] = useState('');

 

  if(serverAddress=='') {
    return <ServerAddress originalServerAddress='localhost:8082' setServerAddress={setNewServerAddress} />
  }
  if(username=='') {
    return <UserName setUsername={setUsername} />
  }
  return (
    <>
      <VoteScreen serverAddress={serverAddress} username={username} />
    </>
  )
}

export default App
