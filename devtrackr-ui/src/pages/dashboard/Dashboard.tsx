import ProfileCard from "./ProfileCard";
import AiReportCard from "./AiReportCard";
import GithubCard from "./GithubCard";
import TaskCard from "./TaskCard";
import PomodoroCard from "./PomodoroCard";
import StatsCard from "./StatsCard";

import "./Dashboard.css";

const Dashboard = () => {
    return (
        <div className="dashboard-grid">
            <ProfileCard />
            <AiReportCard />
            <GithubCard />
            <TaskCard />
            <PomodoroCard />
            <StatsCard />
        </div>
    );
};

export default Dashboard;
