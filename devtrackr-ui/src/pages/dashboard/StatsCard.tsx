import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import "./DashboardCard.css";

const StatsCard = () => {
    const { token, user } = useAuth();
    const [stats, setStats] = useState<any>(null);

    useEffect(() => {
        if (!token || !user) return;

        const headers = { Authorization: `Bearer ${token}` };

        const getData = async () => {
            try {
                const [taskRes, pomoRes, activityRes] = await Promise.all([
                    axios.get(`http://localhost:5103/api/Task/${user.id}/completed`, { headers }),
                    axios.get(`http://localhost:5104/api/Pomodoro/${user.id}/completed`, { headers }),
                    axios.get(`http://localhost:5200/api/Activity/summary`, { headers }),
                ]);

                const totalPomodoroMinutes = pomoRes.data.reduce(
                    (sum: number, p: any) => sum + p.duration,
                    0
                );

                setStats({
                    totalTasks: taskRes.data.length,
                    totalPomodoros: pomoRes.data.length,
                    pomodoroMinutes: totalPomodoroMinutes,
                    totalCommits: activityRes.data.totalCommits,
                    repoCount: activityRes.data.repoCount,
                });
            } catch (err) {
                console.error("İstatistikler alınamadı", err);
                setStats(null);
            }
        };

        getData();
    }, [token, user]);

    if (!stats) return <div className="dashboard-card">İstatistikler yükleniyor...</div>;

    return (
        <div className="dashboard-card">
            <h2>📊 Genel İstatistikler</h2>
            <p><strong>Görev Sayısı:</strong> {stats.totalTasks}</p>
            <p><strong>Pomodoro:</strong> {stats.totalPomodoros} oturum ({stats.pomodoroMinutes} dk)</p>
            <p><strong>Commit:</strong> {stats.totalCommits}</p>
            <p><strong>Repo Sayısı:</strong> {stats.repoCount}</p>
        </div>
    );
};

export default StatsCard;
