import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import "./DashboardCard.css";

const PomodoroCard = () => {
    const { token, user } = useAuth();
    const [pomodoros, setPomodoros] = useState<any[]>([]);

    useEffect(() => {
        if (!token || !user) return;

        axios
            .get(`http://localhost:5104/api/Pomodoro/${user.id}/completed`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((res) => setPomodoros(res.data))
            .catch((err) => {
                console.error("Pomodoro verisi alınamadı", err);
                setPomodoros([]);
            });
    }, [token, user]);

    return (
        <div className="dashboard-card">
            <h2>⏱️ Tamamlanan Pomodorolar</h2>
            {pomodoros.length === 0 ? (
                <p>Henüz tamamlanan pomodoro yok.</p>
            ) : (
                    <ul>
                        {pomodoros.slice(0, 5).map((p, idx) => (
                            <li key={idx}>{p}</li> // p zaten string (örnek: "Focused 25 min, Break 5 min on 2025-05-15 15:00")
                        ))}
                    </ul>
            )}
        </div>
    );
};

export default PomodoroCard;
