import { getCatalogPaginationServices } from "@/services/server/catalogServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request:NextRequest){
    const searchParams = request.nextUrl.searchParams;
    const page = searchParams.get("page");
    const response = await getCatalogPaginationServices(page? parseInt(page): 1);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message:response.data.message
        },{
            status:response.status
        })
    }
    return NextResponse.json(response.data);
}