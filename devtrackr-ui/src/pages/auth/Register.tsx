import { useState } from "react";
import { register } from "../../api/authApi";
import { useNavigate, Link } from "react-router-dom";
import "./AuthForm.css";

const Register = () => {
    const [form, setForm] = useState({
        email: "",
        fullName: "",
        gitHubUsername: "",
        password: "",
    });

    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await register(form);
            navigate("/auth/login");
        } catch (err) {
            alert("Kay�t ba�ar�s�z!");
            console.error(err);
        }
    };

    return (
        <form className="auth-form" onSubmit={handleSubmit}>
            <h2>Kay�t Ol</h2>
            <input name="fullName" placeholder="Ad Soyad" onChange={handleChange} required />
            <input name="email" placeholder="Email" onChange={handleChange} required />
            <input name="gitHubUsername" placeholder="GitHub Kullan�c� Ad�" onChange={handleChange} required />
            <input name="password" type="password" placeholder="�ifre" onChange={handleChange} required />
            <button type="submit">Kay�t Ol</button>
            <p>Zaten bir hesab�n var m�? <Link to="/auth/login">Giri� Yap</Link></p>

        </form>
    );
};

export default Register;
