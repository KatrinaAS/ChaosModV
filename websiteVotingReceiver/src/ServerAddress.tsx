import { useState } from 'react';
export interface ServerAddressProps {
    originalServerAddress: string
    setServerAddress: (serverAddress: string) => void
}

const ServerAddress = ({originalServerAddress, setServerAddress}: ServerAddressProps) => {
    const [serverAddress, setNewServerAddress] = useState(originalServerAddress);

    const checkServerAddress = () => {
        if(serverAddress.length > 0) {
            setServerAddress(serverAddress);
        }
    }
    return ( <div>
        <label>
            Server Address:
            <input type="text" value={serverAddress} onChange={(e) => setNewServerAddress(e.target.value)} />
        </label>
        <button onClick={() => checkServerAddress()} disabled={serverAddress.length==0} >Save</button>
    </div>)


}

export default ServerAddress;