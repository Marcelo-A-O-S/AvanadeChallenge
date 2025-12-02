'use client';
import { ResponseAuth } from "@/types/response-data";
import { User } from "@/types/user";
import { createContext, ReactNode, useContext, useEffect, useState } from "react";
import { toast } from "sonner";
import { useRouter } from "next/navigation";
type AuthProviderProps = {
    children: ReactNode,
    InitialUser?: User | null
};
type AuthContextType = {
    user: User | null
    login: (email: string, password: string) => Promise<ResponseAuth>,
    register: (name: string, email: string, password: string, passwordConfirm: string) => Promise<ResponseAuth>,
    logout: () => void
};
export const AuthContext = createContext<AuthContextType | undefined>(undefined);
export function AuthProvider({ children }: AuthProviderProps) {
    const router = useRouter();
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);
    useEffect(() => {
        checkAuth();
    }, [])
    const login = async (email: string, password: string): Promise<ResponseAuth> => {
        setLoading(true);
        try {
            const response = await fetch("/api/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ email, password })
            });
            const data = await response.json();
            if (response.ok) {
                await checkAuth();
                return {
                    status: response.status,
                    data: { message: "Login realizado com sucesso!" }
                };
            }
            return {
                status: response.status,
                data: data.message
            };
        } catch (err) {
            return {
                status: 500,
                data: {
                    message: (err as Error).message
                }
            };
        } finally {
            setLoading(false);
        }
    }
    const register = async (name: string, email: string, password: string, passwordConfirm: string): Promise<ResponseAuth> => {
        const response = await fetch("/api/auth/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ name, email, password, passwordConfirm })
        });
        const data = await response.json();
        return {
            status: response.status,
            data: data
        };
    }
    const checkAuth = async () => {
        try {
            const response = await fetch("/api/auth/me",
                {
                    method: "GET",
                    headers:
                        { "Content-Type": "application/json" }
                });
            if (response.ok) {
                const userData = await response.json();
                const normalizedUser = {
                    name: userData.user.name,
                    email: userData.user.email,
                    role: userData.user.role,
                };
                setUser(normalizedUser);
            } else {
                setUser(null)
            }
        } catch (error) {
            console.log("Verificação de autenticação retornou uma falha:", error);
            setUser(null);
            setLoading(false);
        } finally {
            console.log("USER ATUALIZADO:", user);
            setLoading(false);
        }
    }
    const logout = async () => {
        const response = await fetch("/api/auth/logout", {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        });
        const data = await response.json();
        if (response.status !== 200) {
            console.log(data.message);
            return;
        }
        await checkAuth();
        setUser(null);
        toast.success(data.message);
        router.push("/auth/login");
        router.refresh();
    }


    return (
        <>
            <AuthContext.Provider value={{ user, login, register, logout }}>
                {children}
            </AuthContext.Provider>
        </>
    )
}
export function useAuth() {
    const context = useContext(AuthContext);
    if (context == undefined) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
}