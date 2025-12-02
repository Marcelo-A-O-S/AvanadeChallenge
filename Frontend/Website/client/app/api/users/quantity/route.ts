import { getUsersQuantity } from "@/services/server/userServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    const response = await getUsersQuantity();
    console.log(response.data);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}