import { getOrderQuantity, getOrderQuantityByUserId } from "@/services/server/orderServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    const searchParams = request.nextUrl.searchParams;
    const userIdValue = searchParams.get("userId");
    let response = null;
    if(userIdValue){
        const userId = userIdValue? parseInt(userIdValue) : 1;
        response = await getOrderQuantityByUserId(userId);
    }else{
        response = await getOrderQuantity();
    }
    
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}