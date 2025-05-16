import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";
import axios from "axios";
import "./ReportList.css";

const ReportList = () => {
    const { token } = useAuth();
    const [reports, setReports] = useState<
        { summary: string; generatedAt: string }[]
    >([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!token) return;

        axios
            .get("http://localhost:5300/api/AiReport/me", {
                headers: { Authorization: `Bearer ${token}` },
            })
            .then((res) => {
                const data = res.data;

                const validReports = data
                    .filter((r: any) => r.summary && !r.summary.includes("Unauthorized"))
                    .sort(
                        (a: any, b: any) =>
                            new Date(b.generatedAt).getTime() - new Date(a.generatedAt).getTime()
                    );

                setReports(
                    validReports.map((r: any) => ({
                        summary: r.summary,
                        generatedAt: new Date(r.generatedAt).toLocaleDateString("tr-TR", {
                            day: "numeric",
                            month: "long",
                            year: "numeric",
                        }),
                    }))
                );
            })
            .catch((err) => {
                console.error("❌ Raporlar alınamadı:", err);
                setReports([]);
            })
            .finally(() => {
                setLoading(false);
            });
    }, [token]);

    return (
        <div className="report-list-container">
            <h1 className="report-title">📝 AI Rapor Geçmişi</h1>
            {loading ? (
                <p>Yükleniyor...</p>
            ) : reports.length === 0 ? (
                <p>Henüz kayıtlı AI raporu bulunamadı.</p>
            ) : (
                <div className="report-list">
                    {reports.map((report, idx) => (
                        <div key={idx} className="report-card">
                            <div className="report-date">{report.generatedAt}</div>
                            <p className="report-summary">{report.summary}</p>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default ReportList;
