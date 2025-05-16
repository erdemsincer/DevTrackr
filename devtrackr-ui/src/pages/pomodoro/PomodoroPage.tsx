import { useEffect, useState } from "react";
import axios from "axios";
import { useAuth } from "../../context/AuthContext";
import PomodoroTimer from "./PomodoroTimer";


import "./PomodoroPage.css";

const PomodoroPage = () => {
    const { token } = useAuth();
    const [sessions, setSessions] = useState<any[]>([]);
    const [focusMinutes, setFocusMinutes] = useState(25);
    const [breakMinutes, setBreakMinutes] = useState(5);
    const [loading, setLoading] = useState(true);

    const [activeSessionId, setActiveSessionId] = useState<number | null>(null);
    const [activeFocusMinutes, setActiveFocusMinutes] = useState<number>(0);

    const api = axios.create({
        baseURL: "http://localhost:5104/api/Pomodoro",
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });

    const fetchSessions = async () => {
        try {
            const res = await api.get("/");
            setSessions(res.data);
        } catch (err) {
            console.error("❌ Oturumlar alınamadı", err);
        } finally {
            setLoading(false);
        }
    };

    const startSession = async () => {
        try {
            const res = await api.post("/", { focusMinutes, breakMinutes });
            setActiveSessionId(res.data.id);
            setActiveFocusMinutes(res.data.focusMinutes);
            fetchSessions();
        } catch (err) {
            console.error("❌ Başlatılamadı", err);
        }
    };

    const completeSession = async (id: number) => {
        try {
            await api.put(`/${id}/complete`, {});
            setActiveSessionId(null); // sayaç sıfırlansın
            fetchSessions();
        } catch (err) {
            console.error("❌ Tamamlanamadı", err);
        }
    };

    useEffect(() => {
        if (token) fetchSessions();
    }, [token]);

    return (
        <div className="pomodoro-container">
            <h1 className="page-title">⏳ Pomodoro Oturumları</h1>

            <div className="start-form">
                <input
                    type="number"
                    value={focusMinutes}
                    onChange={(e) => setFocusMinutes(Number(e.target.value))}
                    placeholder="Odak süresi (dk)"
                />
                <input
                    type="number"
                    value={breakMinutes}
                    onChange={(e) => setBreakMinutes(Number(e.target.value))}
                    placeholder="Mola süresi (dk)"
                />
                <button onClick={startSession}>Yeni Pomodoro Başlat</button>
            </div>

            {activeSessionId && (
                <PomodoroTimer
                    sessionId={activeSessionId}
                    focusMinutes={activeFocusMinutes}
                    onComplete={completeSession}
                />
            )}

            <div className="session-list">
                {loading ? (
                    <p>Yükleniyor...</p>
                ) : sessions.length === 0 ? (
                    <p>Hiç pomodoro oturumu yok.</p>
                ) : (
                    sessions.map((s) => (
                        <div key={s.id} className="session-card">
                            <div>
                                <strong>⏱ {s.focusMinutes}dk / ☕ {s.breakMinutes}dk</strong>
                                <p>Başlangıç: {new Date(s.startTime).toLocaleString("tr-TR")}</p>
                                {s.endTime && (
                                    <p>Bitiş: {new Date(s.endTime).toLocaleString("tr-TR")}</p>
                                )}
                                <p>
                                    Durum:{" "}
                                    {s.isCompleted ? (
                                        <span className="completed">✅ Tamamlandı</span>
                                    ) : (
                                        <span className="pending">⏳ Devam Ediyor</span>
                                    )}
                                </p>
                            </div>

                            {!s.isCompleted && (
                                <button onClick={() => completeSession(s.id)}>
                                    Oturumu Bitir
                                </button>
                            )}
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default PomodoroPage;
