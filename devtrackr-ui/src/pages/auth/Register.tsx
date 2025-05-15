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
            alert("Kayýt baþarýsýz!");
            console.error(err);
        }
    };

    return (
        <form className="auth-form" onSubmit={handleSubmit}>
            <h2>Kayýt Ol</h2>
            <input name="fullName" placeholder="Ad Soyad" onChange={handleChange} required />
            <input name="email" placeholder="Email" onChange={handleChange} required />
            <input name="gitHubUsername" placeholder="GitHub Kullanýcý Adý" onChange={handleChange} required />
            <input name="password" type="password" placeholder="Þifre" onChange={handleChange} required />
            <button type="submit">Kayýt Ol</button>
            <p>Zaten bir hesabýn var mý? <Link to="/auth/login">Giriþ Yap</Link></p>

        </form>
    );
};

export default Register;
