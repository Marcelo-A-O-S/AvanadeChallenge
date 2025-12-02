import { productSchema } from "@/schemas/ProductSchema";
import { addProductService, deleteProductService, getProducts, getProductsPagination } from "@/services/server/productServices";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest){
    const data = await request.json();
    const result = await productSchema.safeParseAsync(data)
    if(result.error){
        return NextResponse.json({
            message: "Error ao validar informações, corrija para prosseguir"
        },{
            status: 400
        })
    }
    const product = result.data;
    const response = await addProductService(product);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message:response.data.message
        },{
            status: response.status
        })
    }
    return NextResponse.json({
        message: "Produto salvo com sucesso"
    })
}
export async function GET(request: NextRequest){
    const searchParams = request.nextUrl.searchParams;
    const page = searchParams.get("page");
    let response = null;
    if(page){
        response = await getProductsPagination(parseInt(page));
    }else{
        response = await getProducts();
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
export async function PUT(request: NextRequest){
    const data = await request.json();
    const result = await productSchema.safeParseAsync(data)
    if(result.error){
        return NextResponse.json({
            message: "Error ao validar informações, corrija para prosseguir"
        },{
            status: 400
        })
    }
    const product = result.data;
}
export async function DELETE(request: NextRequest){
    const searchParams = request.nextUrl.searchParams;
    const Id = searchParams.get("Id");
    if(!Id) return NextResponse.json({
        message:"Identificador inválido"
    },{
        status:400
    })
    const response = await deleteProductService(parseInt(Id));
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message:response.data.message
        },{
            status:response.status
        })
    }
    return NextResponse.json(response.data.message)
}