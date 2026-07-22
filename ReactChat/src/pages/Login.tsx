import { useState } from 'react';

interface LoginProps {
    onLogin: (user: string, pass: string) => Promise<void>;
    errorMsg: string;
    successMsg: string;
}

export function Login({ onLogin, errorMsg, successMsg }: LoginProps) {
    const [loginUser, setLoginUser] = useState("");
    const [loginPass, setLoginPass] = useState("");

    return (
        <div className="max-w-md mx-auto mt-12 bg-white border border-gray-100 shadow-xl rounded-2xl p-8">
            <h3 className="text-2xl font-bold text-gray-900 mb-2">Welcome Back</h3>
            <p className="text-sm text-gray-500 mb-6">Sign in to join the live global chatroom.</p>

            
            <div className="space-y-4 mb-6">
                <div>
                    <label className="block text-xs uppercase tracking-wider font-semibold text-gray-500 mb-2">Username</label>
                    <input type="text" value={loginUser} onChange={e => setLoginUser(e.target.value)} className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-500 transition-all" placeholder="Enter your username..." />
                </div>
                <div>
                    <label className="block text-xs uppercase tracking-wider font-semibold text-gray-500 mb-2">Password</label>
                    <input type="password" value={loginPass} onChange={e => setLoginPass(e.target.value)} className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500/20 focus:border-indigo-500 transition-all" />
                </div>
            </div>

            <button onClick={() => onLogin(loginUser, loginPass)} className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 px-4 rounded-xl shadow-md shadow-indigo-500/10 transition-all active:scale-[0.98]">
                Sign In
            </button>     


            {(errorMsg || successMsg) && (
                <div className="fixed top-5 right-5 z-50 animate-[bounce_1s_ease-out_4]">
                    <div className={`px-4 py-3 rounded-xl shadow-2xl border text-sm font-medium flex items-center gap-2 ${errorMsg
                            ? 'bg-red-500 text-white border-red-600'
                            : 'bg-emerald-500 text-white border-emerald-600'
                        }`}>
                        <span>{errorMsg ? '⚠️' : '✓'}</span>
                        <span>{errorMsg || successMsg}</span>
                    </div>
                </div>
            )}
        </div>

    );
}