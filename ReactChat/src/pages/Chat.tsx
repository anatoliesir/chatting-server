import { useState, useRef, useEffect} from 'react';
import type { GlobalChat } from '../types/chat.types'; // S-a adăugat cuvântul 'type'

interface ChatProps {
    username: string;
    messages: GlobalChat[];
    isConnected: boolean;
    inputText: string;
    setInputText: (text: string) => void;
    onSendMessage: () => void;
    onLogout: () => void;
    onDeleteAccount: () => void;
    onClearAllMessages: () => void;
}

export function Chat({
    username,
    messages,
    isConnected,
    inputText,
    setInputText,
    onSendMessage,
    onLogout,
    onDeleteAccount,
    onClearAllMessages
}: ChatProps) {
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
    const [deleteConfirmationText, setDeleteConfirmationText] = useState("");
    const chatContainerRef = useRef<HTMLDivElement>(null);

    const [isUserScrolledUp, setIsUserScrolledUp] = useState(false);

    // Funcție directă pentru scroll jos
    const scrollToBottom = () => {
        if (chatContainerRef.current) {
            chatContainerRef.current.scrollTo({
                top: chatContainerRef.current.scrollHeight,
                behavior: 'smooth'
            });
        }
    };

    // 1. Detectăm când utilizatorul mișcă de scrollbar
    const handleScroll = () => {
        if (!chatContainerRef.current) return;

        const { scrollTop, scrollHeight, clientHeight } = chatContainerRef.current;
        const distanceFromBottom = scrollHeight - scrollTop - clientHeight;

        // Dacă e mai sus de 100px față de margine, considerăm că s-a "dezlipit" de jos
        if (distanceFromBottom > 100) {
            setIsUserScrolledUp(true);
        } else {
            setIsUserScrolledUp(false);
        }
    };

    // 2. Când vine un mesaj nou în [messages]
    useEffect(() => {
        if (!chatContainerRef.current) return;

        const { scrollTop, scrollHeight, clientHeight } = chatContainerRef.current;
        const distanceFromBottom = scrollHeight - scrollTop - clientHeight;

        // Facem scroll automat DOAR dacă utilizatorul era deja jos (distanță < 150px)
        if (distanceFromBottom < 150) {
            scrollToBottom();
        }
    }, [messages]);



    // Go to the latest messages when logging in.
    useEffect(() => {
        if (!chatContainerRef.current) return;

        setTimeout(() => scrollToBottom(), 200)
    }, [isConnected]);

    return (
        <div className="max-w-4xl mx-auto bg-white border border-gray-100 shadow-xl rounded-2xl overflow-hidden flex flex-col h-[650px]">
            <div className="bg-gray-900 text-white px-6 py-4 flex items-center justify-between">
                <div>
                    <h3 className="font-bold text-lg tracking-tight">Global Chat</h3>
                    <p className="text-xs text-gray-400 flex items-center gap-1.5 mt-0.5">
                        <span className={`w-2 h-2 rounded-full ${isConnected ? 'bg-green-500' : 'bg-amber-500'}`}></span>
                        {isConnected ? `Logged in as: ${username}` : 'Connecting to hub...'}
                    </p>
                </div>
                <button onClick={onLogout} className="text-xs font-semibold bg-white/10 hover:bg-white/20 px-3 py-1.5 rounded-lg transition-all">
                    Logout
                </button>
            </div>

            <div ref={chatContainerRef}
                onScroll={handleScroll}
                className="flex-grow p-6 overflow-y-auto bg-gray-50/60 flex flex-col gap-3">
                {messages.map((msg, index) => {
                    const isMe = msg.userName === username;
                    return (
                        <div key={index} className={`flex flex-col max-w-[75%] ${isMe ? 'self-end items-end' : 'self-start items-start'}`}>
                            <span className="text-xs text-gray-400 font-medium mb-1 px-1">{msg.userName}</span>
                            <div className={`px-4 py-2.5 rounded-2xl text-sm shadow-sm leading-relaxed ${isMe ? 'bg-indigo-600 text-white rounded-tr-none' : 'bg-white text-gray-800 border border-gray-200/80 rounded-tl-none'}`}>
                                {msg.message}
                            </div>
                            <span className="text-[10px] text-gray-400 mt-1 px-1">
                                {msg.sentAt ? new Date(msg.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : ''}
                            </span>
                        </div>
                    );
                })}                
            </div>

            <div className="p-4 bg-white border-t border-gray-100 flex gap-2 items-center">
                <input type="text" value={inputText} onChange={e => setInputText(e.target.value)} onKeyDown={e => e.key === 'Enter' && onSendMessage()} placeholder="Write a message..." className="flex-grow px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl focus:outline-none focus:border-indigo-500 transition-all text-sm" />
                <button onClick={onSendMessage} disabled={!isConnected || !inputText.trim()} className="bg-indigo-600 hover:bg-indigo-700 disabled:opacity-40 disabled:hover:bg-indigo-600 text-white font-semibold text-sm px-6 py-3 rounded-xl shadow-md shadow-indigo-500/5 transition-all active:scale-[0.98]">
                    Send
                </button>
            </div>

            <div className="p-6 bg-red-50/60 border-t border-red-100">
                <h4 className="text-sm font-bold text-red-700 uppercase tracking-wider mb-3">Danger Zone</h4>
                {!showDeleteConfirmation ? (
                    <div className="flex flex-wrap gap-2">
                        <button onClick={() => setShowDeleteConfirmation(true)} className="bg-red-600 hover:bg-red-700 text-white text-xs font-semibold py-2 px-4 rounded-lg shadow-sm transition-all">
                            {username === "admin" ? "Delete My Account / Clear All Data" : "Delete My Account"}
                        </button>
                    </div>
                ) : (
                    <div className="bg-white border border-red-200 rounded-xl p-4 text-sm shadow-sm">
                        <p className="text-gray-700 mb-3">This action is <strong className="text-red-600 font-bold">permanent</strong>. To confirm, type your username (<strong className="font-semibold">{username}</strong>):</p>
                        <input type="text" value={deleteConfirmationText} onChange={e => setDeleteConfirmationText(e.target.value)} placeholder="Type your username to confirm..." className="w-full px-3 py-2 border border-gray-200 rounded-lg text-sm mb-3 focus:outline-none focus:border-red-500" />

                        <div className="flex gap-2">
                            <button onClick={onDeleteAccount} disabled={deleteConfirmationText !== username} className="bg-red-600 hover:bg-red-700 disabled:opacity-40 text-white text-xs font-semibold py-2 px-4 rounded-lg transition-all">
                                Confirm Permanent Deletion
                            </button>
                            <button onClick={() => { setShowDeleteConfirmation(false); setDeleteConfirmationText(""); }} className="bg-gray-500 hover:bg-gray-600 text-white text-xs font-semibold py-2 px-4 rounded-lg transition-all">
                                Cancel
                            </button>
                            {username === "admin" && (
                                <button onClick={onClearAllMessages} className="bg-amber-600 hover:bg-amber-700 text-white text-xs font-semibold py-2 px-4 rounded-lg transition-all">
                                    Delete all messages (Admin Only)
                                </button>
                            )}
                        </div>
                    </div>
                )}
            </div>
            {isUserScrolledUp && (
                <button
                    onClick={scrollToBottom}
                    className="absolute bottom-20 right-8 bg-indigo-600 text-white text-xs py-1.5 px-3 rounded-full shadow-lg hover:bg-indigo-700 transition-all flex items-center gap-1"
                >
                    ↓ Go to new messages
                </button>
            )}
        </div>
    );
}