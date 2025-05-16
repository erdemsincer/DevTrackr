import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import "./Navbar.css";

const Navbar = () => {
    const { isAuthenticated, logout, user } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/auth/login");
    };

    return (
        <nav className="navbar">
            <Link to="/" className="logo">DevTrackr</Link>

            <div className="nav-links">
                <Link to="/">Dashboard</Link>
                <Link to="/reports">Raporlar</Link>
                <Link to="/pomodoro">Pomodoro</Link> {/* ✅ EKLENDİ */}

                {isAuthenticated ? (
                    <>
                        <span className="username">👋 {user?.name}</span>
                        <button onClick={handleLogout} className="logout-button">
                            Çıkış Yap
                        </button>
                    </>
                ) : (
                    <>
                        <Link to="/auth/login">Giriş Yap</Link>
                        <Link to="/auth/register">Kayıt Ol</Link>
                    </>
                )}
            </div>
        </nav>
    );
};

export default Navbar;
