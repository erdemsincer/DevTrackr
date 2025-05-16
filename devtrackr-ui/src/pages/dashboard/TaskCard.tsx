import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import "./DashboardCard.css";

const TaskCard = () => {
    const { token, user } = useAuth();
    const [tasks, setTasks] = useState<any[]>([]);

    useEffect(() => {
        if (!token || !user) return;

        axios
            .get(`http://localhost:5103/api/Task/${user.id}/completed`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((res) => setTasks(res.data))
            .catch((err) => {
                console.error("Görev verileri alınamadı", err);
                setTasks([]);
            });
    }, [token, user]);

    return (
        <div className="dashboard-card">
            <h2>✅ Tamamlanan Görevler</h2>
            {tasks.length === 0 ? (
                <p>Henüz tamamlanan görev yok.</p>
            ) : (
                    <ul>
                        {tasks.slice(0, 5).map((task, idx) => (
                            <li key={idx}>{task}</li> // ✅ task bir string zaten
                        ))}
                    </ul>
            )}
        </div>
    );
};

export default TaskCard;
