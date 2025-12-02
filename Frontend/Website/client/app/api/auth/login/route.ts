import { autenticateUser, verifyToken } from "@/lib/auth";
import { loginSchema } from "@/schemas/LoginSchema";
import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
export async function POST(request: NextRequest) {
    try {
        const data = await request.json();
        const result  = await loginSchema.safeParseAsync(data);
        if(result.error){
            return NextResponse.json({
                message:result.error.message
            },{
                status: 400
            })
        }
        const {email,password} = result.data;
        const response = await autenticateUser(email, password);
        if (response.status != 200) {
            return NextResponse.json(
                { message: response.data.message},
                { status: response.status }
            )
        }
        const token = `${response.data.message}`;
        const user = await verifyToken(token);
        if(!user){
            return NextResponse.json(
                { message: "Token inválido."},
                { status: 401 }
            )
        }
        const cookieStore = await cookies();
        cookieStore.set({
            name: "auth-token",
            value: token,
            path: "/",
            secure: process.env.NODE_ENV === "production",
            maxAge: 60 * 60 * 24
        })
        return NextResponse.json({user});;
    } catch (err) {
        return NextResponse.json(
            {message: ((err as Error).message)},
            {status: 500}
        )
    }
}