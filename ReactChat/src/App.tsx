import { useState, useEffect, useRef } from 'react';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import type { GlobalChat } from './types/chat.types'; 
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { Chat } from './pages/Chat';
import { Home } from './pages/Home'

const BASE_URL = "";

function App() {
    const [view, setView] = useState<'home' | 'login' | 'register' | 'chat'>('home');
    const [username, setUsername] = useState<string | null>(null);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const [errorMsg, setErrorMsg] = useState("");
    const [successMsg, setSuccessMsg] = useState("");

    const [messages, setMessages] = useState<GlobalChat[]>([]);
    const [inputText, setInputText] = useState("");
    const [isConnected, setIsConnected] = useState(false);
    const hubConnectionRef = useRef<HubConnection | null>(null);

    const changeView = (newView: 'home' | 'login' | 'register' | 'chat') => {
        setErrorMsg("");
        setSuccessMsg("");
        setView(newView);
    };

    const handleRegister = async (user: string, pass: string, passRepeat: string) => {
        setErrorMsg("");
        setSuccessMsg("");

        if (pass !== passRepeat) {
            setErrorMsg("The password must be the same as the repeat one!");
            return;
        }

        try {
            const response = await fetch(`${BASE_URL}/api/user/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username: user, password: pass })
            });

            if (response.ok) {
                setSuccessMsg("Account created successfully! Redirecting to login...");
                setTimeout(() => changeView('login'), 1500);
            } else {
                const text = await response.text();
                setErrorMsg(text || "Registration failed.");
            }
        } catch (ex: any) {
            setErrorMsg(`An error occurred: ${ex.message}`);
        }
    };

    const handleLogin = async (user: string, pass: string) => {
        setErrorMsg("");
        setSuccessMsg("");

        try {
            const response = await fetch(`${BASE_URL}/api/user/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username: user, password: pass })
            });

            if (response.ok) {
                const text = await response.text();
                console.log(text);
                setSuccessMsg(text);

                setUsername(user);
                setIsLoggedIn(true);
                
                setTimeout(() => changeView('chat'), 1500);
            } else {
                const text = await response.text();
                setErrorMsg(text || "Invalid credentials.");
            }
        } catch (ex: any) {
            setErrorMsg("Cannot connect to server. Please check the backend connection.");
        }
    };

    const handleLogout = () => {
        setIsLoggedIn(false);
        setUsername(null);
        if (hubConnectionRef.current) {
            hubConnectionRef.current.stop();
        }
        changeView('login');
    };

    useEffect(() => {
        if (view !== 'chat' || !isLoggedIn) return;

        fetch(`${BASE_URL}/api/globalchat`)
            .then(res => res.json())
            .then(data => setMessages(data))
            .catch(err => console.error("Error loading chat history:", err));

        const connection = new HubConnectionBuilder()
            .withUrl(`${BASE_URL}/chathub`)
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveMessage", (liveMessage: GlobalChat) => {
            setMessages(prev => prev.concat(liveMessage));
        });

        connection.on("ChatCleared", () => {
            setMessages([]);
        });

        connection.start()
            .then(() => {
                setIsConnected(true);
                console.log("Connected to SignalR Hub!");
            })
            .catch(err => console.error("SignalR Connection Error: ", err));

        hubConnectionRef.current = connection;

        return () => {
            if (connection.state === HubConnectionState.Connected) {
                connection.stop();
            }
        };
    }, [view, isLoggedIn]);

    const trimiteMesaj = async () => {
        if (hubConnectionRef.current && isConnected && inputText.trim() !== "") {
            try {
                await hubConnectionRef.current.send("SendMessage", username, inputText);
                setInputText("");
            } catch (err) {
                console.error("Error sending message:", err);
            }
        }
    };

    const executeAccountDeletion = async () => {
        try {
            const response = await fetch(`${BASE_URL}/api/user/delete/${username}`, { method: 'DELETE' });
            if (response.ok) {
                handleLogout();
            } else {
                console.error("Deletion failed");
            }
        } catch (ex) {
            console.error("Network error during deletion", ex);
        }
    };

    const clearAllMessages = async () => {
        try {
            await fetch(`${BASE_URL}/api/globalchat/clear-all?username=${username}`, { method: 'DELETE' });
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <div className="min-h-screen bg-gradient-to-tr from-slate-50 via-gray-50 to-indigo-50/30 text-gray-800 font-sans antialiased">
            <nav className="bg-white/80 backdrop-blur-md border-b border-gray-200/80 sticky top-0 z-50 transition-all">
                <div className="max-w-6xl mx-auto px-6 h-16 flex items-center justify-between">
                    <div onClick={() => changeView('home')} className="text-xl font-extrabold tracking-tight text-transparent bg-clip-text bg-gradient-to-r from-indigo-600 to-indigo-950 cursor-pointer hover:opacity-85 transition-all">
                        ReactChat
                    </div>
                    <div className="flex items-center gap-6 text-sm font-semibold text-gray-600">
                        <span onClick={() => changeView('home')} className={`cursor-pointer transition-colors hover:text-indigo-600 ${view === 'home' ? 'text-indigo-600' : ''}`}>Home</span>

                        {!isLoggedIn ? (
                            <>
                                <span onClick={() => changeView('login')} className={`cursor-pointer transition-colors hover:text-indigo-600 ${view === 'login' ? 'text-indigo-600' : ''}`}>Login</span>
                                <span onClick={() => changeView('register')} className={`px-3.5 py-1.5 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-all shadow-sm shadow-indigo-500/10 cursor-pointer`}>Register</span>
                            </>
                        ) : (
                            <span onClick={() => changeView('chat')} className={`px-3.5 py-1.5 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-all shadow-sm shadow-indigo-500/10 cursor-pointer ${view === 'chat' ? 'bg-indigo-700' : ''}`}>Global Chat</span>
                        )}
                    </div>
                </div>
            </nav>

            <main className="max-w-6xl mx-auto px-6 py-10">
                {view === 'home' && <Home />}
                {view === 'login' && <Login onLogin={handleLogin} errorMsg={errorMsg} successMsg={successMsg} />}
                {view === 'register' && <Register onRegister={handleRegister} errorMsg={errorMsg} successMsg={successMsg} changeView={changeView} />}
                {view === 'chat' && (
                    <Chat
                        username={username || ""}
                        messages={messages}
                        isConnected={isConnected}
                        inputText={inputText}
                        setInputText={setInputText}
                        onSendMessage={trimiteMesaj}
                        onLogout={handleLogout}
                        onDeleteAccount={executeAccountDeletion}
                        onClearAllMessages={clearAllMessages}
                    />
                )}
            </main>
        </div>
    );
}

export default App;