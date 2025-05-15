import { useState } from "react";
import { login as loginRequest } from "../../api/authApi";
import { useAuth } from "../../context/AuthContext";
import { useNavigate, Link } from "react-router-dom";
import "./AuthForm.css";

const Login = () => {
    const [form, setForm] = useState({ email: "", password: "" });
    const { login: saveToken } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const res = await loginRequest(form); // ✅ değiştirildi
            saveToken(res.data.token);
            navigate("/");
        } catch (err) {
            alert("Giriş başarısız!");
            console.error(err);
        }
    };

    return (
        <form className="auth-form" onSubmit={handleSubmit}>
            <h2>Giriş Yap</h2>
            <input type="email" name="email" placeholder="Email" onChange={handleChange} required />
            <input type="password" name="password" placeholder="Şifre" onChange={handleChange} required />
            <button type="submit">Giriş Yap</button>
            <p>Hesabın yok mu? <Link to="/auth/register">Kayıt Ol</Link></p>
        </form>
    );
};

export default Login;
