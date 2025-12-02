import { getProductQuantity } from "@/services/server/productServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    const response = await getProductQuantity();
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }

    return NextResponse.json(response.data);
}