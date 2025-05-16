import { useNavigate } from "react-router-dom";

import ProfileCard from "./ProfileCard";
import AiReportCard from "./AiReportCard";
import GithubCard from "./GithubCard";
import TaskCard from "./TaskCard";
import PomodoroCard from "./PomodoroCard";
import StatsCard from "./StatsCard";

import "./Dashboard.css";

const Dashboard = () => {
    const navigate = useNavigate();

    return (
        <>
            <h1 className="page-title">📊 Dashboard</h1>

            <div className="dashboard-grid">
                <ProfileCard />
                <AiReportCard />
                <GithubCard />
                <TaskCard />
                <PomodoroCard />
                <StatsCard />

                <div className="report-button-wrapper">
                    <button
                        className="go-report-button"
                        onClick={() => navigate("/reports")}
                    >
                        Tüm AI Raporlarını Gör
                    </button>
                </div>
            </div>
        </>
    );
};

export default Dashboard;
