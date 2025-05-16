import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import "./DashboardCard.css";

const ProfileCard = () => {
    const { token } = useAuth();
    const [profile, setProfile] = useState<any>(null);

    useEffect(() => {
        if (!token) return;

        axios
            .get("http://localhost:5102/api/User", {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((res) => setProfile(res.data))
            .catch((err) => console.error("Profil alınamadı", err));
    }, [token]);

    return (
        <div className="dashboard-card">
            <h2>🧑 Kullanıcı Bilgisi</h2>
            {!profile ? (
                <p>Yükleniyor...</p>
            ) : (
                <>
                    <p><strong>Ad Soyad:</strong> {profile.fullName}</p>
                    <p><strong>Email:</strong> {profile.email}</p>
                    <p><strong>GitHub:</strong> {profile.gitHubUsername}</p>
                </>
            )}
        </div>
    );
};

export default ProfileCard;
