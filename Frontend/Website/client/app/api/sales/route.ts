import { saleSchema } from "@/schemas/SaleSchema";
import { addSaleServices, deleteSaleServices, getSalesPaginationService, getSalesService } from "@/services/server/saleServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    const searchParams = request.nextUrl.searchParams;
    const page = searchParams.get("page");
    let response = null;
    if(page){
        response = await getSalesPaginationService(parseInt(page));
    }else{
        response = await getSalesService();
    }
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message:response.data.message
        },{
            status:response.status
        })
    }
    return NextResponse.json(response.data);
}
export async function POST(request: NextRequest){
    const data = await request.json();
    const result = await saleSchema.safeParseAsync(data);
    if(result.error){
        return NextResponse.json({
            message: result.error.message
        },{
            status: 400
        })
    }
    const response = await addSaleServices(result.data);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message:response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json({
        message: "Adicionado no carrinho com sucesso!"
    })
}
export async function DELETE(request: NextRequest){
    const searchParams = request.nextUrl.searchParams;
    const Id = searchParams.get("Id");
    if(!Id) return NextResponse.json({
        message:"Identificador inválido"
    },{
        status:400
    });
    const response = await deleteSaleServices(parseInt(Id));
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message:response.data.message
        },{
            status:response.status
        })
    }
    return NextResponse.json(response.data.message)
}