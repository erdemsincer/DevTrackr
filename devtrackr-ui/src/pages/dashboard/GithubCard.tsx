import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import "./DashboardCard.css";

const GithubCard = () => {
    const { token } = useAuth();
    const [data, setData] = useState<any>(null);

    useEffect(() => {
        if (!token) return;

        axios
            .get("http://localhost:5200/api/Activity/summary", {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((res) => setData(res.data))
            .catch((err) => {
                console.error("GitHub verileri alınamadı", err);
                setData({ error: true });
            });
    }, [token]);

    if (!data) {
        return <div className="dashboard-card">GitHub verileri yükleniyor...</div>;
    }

    if (data.error) {
        return <div className="dashboard-card">GitHub verileri alınamadı.</div>;
    }

    return (
        <div className="dashboard-card">
            <h2>🐙 GitHub Aktivitesi</h2>
            <p><strong>Toplam Commit:</strong> {data.totalCommits}</p>
            <p><strong>Repo Sayısı:</strong> {data.repoCount}</p>
            <p><strong>En Çok Kullanılan Dil:</strong> {data.mostUsedLanguage}</p>
            <p>
                <strong>En Çok Star Alan Repo:</strong> {data.mostStarredRepo} ⭐{" "}
                ({data.mostStars})
            </p>
            <p>
                <strong>Son Push:</strong> {data.lastPushedRepo} –{" "}
                {new Date(data.lastPushDate).toLocaleDateString("tr-TR")}
            </p>
        </div>
    );
};

export default GithubCard;
