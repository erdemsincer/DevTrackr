import { createContext, useContext, useState } from "react";
import { jwtDecode } from "jwt-decode"; // ❌ Bu yanlıştır

interface JwtPayload {
    name: string;
    email: string;
    role: string;
    exp: number;
}

interface AuthContextType {
    token: string | null;
    login: (token: string) => void;
    logout: () => void;
    isAuthenticated: boolean;
    user: JwtPayload | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Token içeriğinden gerçek alanları al
const parseJwtPayload = (raw: any): JwtPayload => ({
    name: raw["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
    email: raw["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
    role: raw["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
    exp: raw["exp"],
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [token, setToken] = useState<string | null>(() => localStorage.getItem("token"));
    const [user, setUser] = useState<JwtPayload | null>(() => {
        const storedToken = localStorage.getItem("token");
        if (!storedToken) return null;
        const raw = jwtDecode<any>(storedToken);
        return parseJwtPayload(raw);
    });

    const login = (newToken: string) => {
        localStorage.setItem("token", newToken);
        setToken(newToken);
        const raw = jwtDecode<any>(newToken);
        setUser(parseJwtPayload(raw));
    };

    const logout = () => {
        localStorage.removeItem("token");
        setToken(null);
        setUser(null);
    };

    const isAuthenticated = !!token;

    return (
        <AuthContext.Provider value={{ token, login, logout, isAuthenticated, user }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) throw new Error("useAuth must be used inside AuthProvider");
    return context;
};
