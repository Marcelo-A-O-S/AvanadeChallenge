import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
export async function GET(request: NextRequest){
    try{
        const cookieStore = await cookies()
        cookieStore.delete("auth-token");
        return NextResponse.json({
                message: "Logout realizado com sucesso, até mais!"
            })
    }catch(err){
        return NextResponse.json({
                message: (err as Error).message
            },{
                status: 500
            })
    }
}