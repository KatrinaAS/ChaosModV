import { useState } from "react";

export interface UserNameProps {
    setUsername: (username: string) => void
}

export const UserName = ({setUsername}: UserNameProps) => {
    const [username, setNewUsername] = useState('');
    const checkUsername = () => { 
        if(username.length > 0) {
            setUsername(username);
        }
    }
    return ( <div>
        <label>
            Username:
            <input type="text" value={username} onChange={(e) => setNewUsername(e.target.value)} />
        </label>
        <button onClick={() => checkUsername()} disabled={username.length==0}>Save</button>
        </div>
    );
}