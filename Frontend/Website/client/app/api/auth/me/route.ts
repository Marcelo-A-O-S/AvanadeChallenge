import { verifyToken } from "@/lib/auth";
import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    try{
        const cookieStore = await cookies();
        const token = cookieStore.get("auth-token")?.value;
        if(!token){
            return NextResponse.json(
                "Usuário não autenticado!",
                {status: 401}
            )
        }
        const user = await verifyToken(token);
        if(!user){
            return NextResponse.json(
                "Token inválido",
                {status: 401}
            )
        }
        return NextResponse.json(
            {user},
        )
    }catch(err){
        return NextResponse.json(
            ((err as Error).message),
            { status: 401 }
        );
    }
}