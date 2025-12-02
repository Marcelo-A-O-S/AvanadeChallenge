import axios from "axios";
const HOST_API = process.env.NEXT_PUBLIC_API_LOCAL || "http://localhost:3000";
export const apiClient = async () => {
    const token = document.cookie.split("; ").find(row => row.startsWith("auth-token="))?.split("=")[1]
    const instance = axios.create({
        baseURL: HOST_API,
        headers: {
            "Content-Type": "application/json",
        },
    });
    instance.interceptors.request.use(
        (config) => {
            if (token) {
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        },
        (error) => {
            return Promise.reject(error);
        }
    );
    return instance;
}