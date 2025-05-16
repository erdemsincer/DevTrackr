import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import "./DashboardCard.css";

const AiReportCard = () => {
    const { token } = useAuth();
    const [lastReport, setLastReport] = useState<{ summary: string; generatedAt: string } | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!token) {
            console.warn("❌ Token bulunamadı, istek atılmadı.");
            return;
        }

        console.log("🚀 Token:", token);

        axios
            .get("http://localhost:5300/api/AiReport/me", {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((res) => {
                const data = res.data;
                console.log("📦 API'den gelen veri:", data);

                if (!data || !Array.isArray(data) || data.length === 0) {
                    console.warn("⚠️ Veri boş veya geçersiz.");
                    setLastReport(null);
                    return;
                }

                // ✅ Unauthorized içermeyen summary'leri filtrele
                const validReports = data.filter(
                    (r: any) => r.summary && !r.summary.includes("Unauthorized")
                );

                if (validReports.length === 0) {
                    console.warn("⚠️ Geçerli rapor bulunamadı.");
                    setLastReport(null);
                    return;
                }

                // 🧠 En güncel generatedAt'e göre sıralayıp ilkini al
                const newestReport = validReports.sort(
                    (a: any, b: any) => new Date(b.generatedAt).getTime() - new Date(a.generatedAt).getTime()
                )[0];

                console.log("🧾 Seçilen en yeni geçerli rapor:", newestReport);

                setLastReport({
                    summary: newestReport.summary,
                    generatedAt: new Date(newestReport.generatedAt).toLocaleDateString("tr-TR", {
                        day: "numeric",
                        month: "long",
                        year: "numeric",
                    }),
                });
            })
            .catch((err) => {
                console.error("❌ API isteği sırasında hata oluştu:", err);
                setLastReport(null);
            })
            .finally(() => {
                setLoading(false);
            });
    }, [token]);

    return (
        <div className="dashboard-card">
            <h2>🤖 Son AI Raporu</h2>
            {loading ? (
                <p>Yükleniyor...</p>
            ) : lastReport ? (
                <div>
                    <strong>{lastReport.generatedAt}</strong>
                    <p style={{ whiteSpace: "pre-wrap", marginTop: "1rem" }}>{lastReport.summary}</p>
                </div>
            ) : (
                <p>Henüz AI raporu yok.</p>
            )}
        </div>
    );
};

export default AiReportCard;
