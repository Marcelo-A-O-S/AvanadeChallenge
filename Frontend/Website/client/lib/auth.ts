import { ResponseAuth } from "@/types/response-data";
import { jwtVerify } from "jose"
interface Payload {
    name: string;
    email: string;
    role: string;
}
const HOST_API = process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5000";
const JWT_SECRET = new TextEncoder().encode(
    process.env.JWT_SECRET || "default_secret_change_this_in_development"
);
export async function verifyToken(token: string): Promise<Payload | null> {
    try {
        const decoded = await jwtVerify(token, JWT_SECRET) as { payload: Payload };
        return decoded.payload;
    } catch (err) {
        return null;
    }
}
export async function autenticateUser(email: string, password: string): Promise<ResponseAuth> {
    const response = await fetch(HOST_API + "/api/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
    });
    const data = await response.json();
    console.log(data);
    return {
        status: response.status,
        data: data
    };
}
export async function registerUser(name: string, email: string, password: string, passwordConfirm: string): Promise<ResponseAuth> {
    const response = await fetch(HOST_API + "/api/auth/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ name, email, password, passwordConfirm })
    })
    const data = await response.json()
    return {
        status: response.status,
        data: data
    };
}