import { getCatalogQuantity } from "@/services/server/catalogServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    const response = await getCatalogQuantity();
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}