import { getUserByEmail } from "@/services/server/userServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    const searchParams = request.nextUrl.searchParams;
    const email = searchParams.get("email");
    if (!email) {
        return NextResponse.json({
            message: "O email não foi informado"
        })
    }
    const response = await getUserByEmail(email);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}