import { registerUser } from "@/lib/auth";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
    try {
        const { name, email, password, passwordConfirm} = await request.json();
        if (!name || !email || !password || !passwordConfirm) {
            return NextResponse.json(
                { message : "Informe as credenciais solicitadas corretamente para prosseguir!"},
                { status: 400 }
            )
        }
        const response = await registerUser(name, email, password, passwordConfirm);
        console.log(response.data);
        return NextResponse.json(
            {message: response.data.message},
            {status : response.status}
        )
    } catch (err) {
        return NextResponse.json({message : ((err as Error).message)}, { status: 500 })
    }
}