import axios from "axios";

// ✅ API instance
const api = axios.create({
    baseURL: "http://localhost:5101/api/Auth",
    headers: {
        "Content-Type": "application/json",
    },
});

// ✅ Register endpoint
export const register = (data: {
    email: string;
    fullName: string;
    gitHubUsername: string;
    password: string;
}) => api.post("/register", data);

// ✅ Login endpoint
export const login = (data: {
    email: string;
    password: string;
}) => api.post("/login", data);

// 🔐 Tokenlı istekler için (ileride)
export const setAuthToken = (token: string | null) => {
    if (token) {
        api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    } else {
        delete api.defaults.headers.common["Authorization"];
    }
};
