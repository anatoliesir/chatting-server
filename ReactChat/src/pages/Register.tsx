import { useState } from 'react';

interface RegisterProps {
    onRegister: (user: string, pass: string, passRepeat: string) => Promise<void>;
    errorMsg: string;
    successMsg: string;
    changeView: (view: 'home' | 'login' | 'register' | 'chat') => void;
}

export function Register({ onRegister, errorMsg, successMsg, changeView }: RegisterProps) {
    const [regUser, setRegUser] = useState("");
    const [regPass, setRegPass] = useState("");
    const [regPassRepeat, setRegPassRepeat] = useState("");

    return (
        <div className="max-w-md mx-auto mt-12 bg-white border border-gray-100 shadow-xl rounded-2xl p-8">
            <h3 className="text-2xl font-bold text-gray-900 mb-2">Create New Account</h3>
            <p className="text-sm text-gray-500 mb-6">Set up your profile credentials below.</p>

            <div className="space-y-4 mb-6">
                <div>
                    <label className="block text-xs uppercase tracking-wider font-semibold text-gray-500 mb-2">Username</label>
                    <input type="text" value={regUser} onChange={e => setRegUser(e.target.value)} className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 transition-all" placeholder="Choose a username..." />
                </div>
                <div>
                    <label className="block text-xs uppercase tracking-wider font-semibold text-gray-500 mb-2">Password</label>
                    <input type="password" value={regPass} onChange={e => setRegPass(e.target.value)} className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 transition-all" />
                </div>
                <div>
                    <label className="block text-xs uppercase tracking-wider font-semibold text-gray-500 mb-2">Repeat password</label>
                    <input type="password" value={regPassRepeat} onChange={e => setRegPassRepeat(e.target.value)} className="w-full px-4 py-2.5 bg-gray-50 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 transition-all" />
                </div>
            </div>

            <button onClick={() => onRegister(regUser, regPass, regPassRepeat)} className="w-full bg-emerald-600 hover:bg-emerald-700 text-white font-semibold py-3 px-4 rounded-xl shadow-md shadow-emerald-500/10 transition-all active:scale-[0.98]">
                Register Account
            </button>

            <div className="mt-6 pt-4 border-t border-gray-100 text-center text-sm text-gray-500">
                Already have an account?{' '}
                <span onClick={() => changeView('login')} className="text-indigo-600 font-semibold cursor-pointer hover:underline">
                    Login here
                </span>
            </div>


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