import axios from "axios";
import { cookies } from "next/headers";
const HOST_API = process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:3001/";
export const apiServer= async () => {
    const cookieStore = await cookies();
    const token = cookieStore.get("auth-token")?.value;

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