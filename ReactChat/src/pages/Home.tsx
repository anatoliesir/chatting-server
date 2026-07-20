// src/pages/Home.tsx

export function Home() {
    return (
        <div className="max-w-3xl mx-auto text-center py-12 px-4">
            <div className="inline-block bg-indigo-50 text-indigo-700 px-4 py-1.5 rounded-full text-sm font-semibold mb-6 tracking-wide shadow-sm">
                Portfolio Full Stack Project
            </div>
            <h1 className="text-5xl font-extrabold text-gray-900 tracking-tight mb-6">
                Hello and Welcome
            </h1>
            <p className="text-xl text-gray-600 leading-relaxed mb-8">
                This is a complex full-stack web application built using <strong className="text-gray-900 font-semibold">ASP.NET Core Web API</strong> on the backend and a fully responsive, modern React frontend styled with Tailwind CSS,
                and real time chat is made with the SignalR WebSocket.
            </p>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-left max-w-2xl mx-auto mb-10">
                <div className="p-5 bg-white border border-gray-100 rounded-xl shadow-sm">
                    <h3 className="font-bold text-gray-800 mb-2">Robust Architecture</h3>
                    <p className="text-sm text-gray-600">Clean Architecture pattern using CQRS decoupled cleanly via MediatR handlers.</p>
                </div>
                <div className="p-5 bg-white border border-gray-100 rounded-xl shadow-sm">
                    <h3 className="font-bold text-gray-800 mb-2">Secure Data Layer</h3>
                    <p className="text-sm text-gray-600">Features secure BCrypt password hashing integrated directly with a PostgreSQL database.</p>
                </div>
            </div>

            <div className="bg-gray-50 border border-gray-200/60 rounded-xl p-6 text-sm text-gray-500 italic max-w-xl mx-auto">
                "To learn and develop these advanced skills, I collaborated closely with Google Gemini because standard online tutorials rarely show production-grade complexity."
            </div>
        </div>
    );
}