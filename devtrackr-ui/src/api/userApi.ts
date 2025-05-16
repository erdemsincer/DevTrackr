import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5102/api/User", // 👈 UserService URL
});

// Token setleme
export const setAuthToken = (token: string | null) => {
    if (token) {
        api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    } else {
        delete api.defaults.headers.common["Authorization"];
    }
};

// Profil çekme
export const getProfile = () => api.get("/");
