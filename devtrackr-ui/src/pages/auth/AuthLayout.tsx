import { Outlet } from "react-router-dom";
import "./AuthLayout.css";

const AuthLayout = () => {
    return (
        <div className="auth-layout">
            <div className="auth-container">
                <h1 className="auth-title">DevTrackr</h1>
                <Outlet />
            </div>
        </div>
    );
};

export default AuthLayout;
