import { getStockMovementByIdService } from "@/services/server/stockServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest, { params }: { params: { Id: string } }){
    const Id = params.Id;
    const response = await getStockMovementByIdService(parseInt(Id));
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message: response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}