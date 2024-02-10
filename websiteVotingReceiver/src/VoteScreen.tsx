import { useEffect, useState } from "react";
import useWebSocket, { ReadyState } from "react-use-websocket";
import { WebsocketMessage } from "./types";
import { Button } from "./Button";

export interface VoteScreenProps {
    serverAddress: string
    username: string
}
const VoteScreen = ({username, serverAddress}:VoteScreenProps) => {
    const [currentMessage, setCurrentMessage] = useState<WebsocketMessage>();
 const openEvent = () => {
      sendMessage(`username~${username}`);
    }
    const { sendMessage, lastMessage, readyState } = useWebSocket("ws://"+serverAddress,{
      onOpen: openEvent,
    });
    

    useEffect(() => {
        if(lastMessage !== null) {
            setCurrentMessage(JSON.parse(lastMessage.data));
        }
    },[lastMessage]);

      const connectionStatus = {
    [ReadyState.CONNECTING]: 'Connecting',
    [ReadyState.OPEN]: 'Open',
    [ReadyState.CLOSING]: 'Closing',
    [ReadyState.CLOSED]: 'Closed',
    [ReadyState.UNINSTANTIATED]: 'Uninstantiated',
  }[readyState];

  const vote = (vote: string) => {
    sendMessage(`vote~${vote}`);
  }

  return (
    <>
    <h1>{connectionStatus}</h1>
      <div className="card">
        {currentMessage && 
        <h2>{currentMessage.options.map(option => <Button text={option.Label} onClick={() => { vote(option.Matches[0])}}/>)}</h2>
    }
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
      </>
  )
}

export default VoteScreen;