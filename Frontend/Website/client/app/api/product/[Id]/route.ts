import { productSchema } from "@/schemas/ProductSchema";
import { getProductByIdService, updateProductService } from "@/services/server/productServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const { Id } = await params;
    const response = await getProductByIdService(parseInt(Id));
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json(response.data)
}
export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }>  }){
    const {Id} = await params;
    const data = await request.json();
    const result = await productSchema.safeParseAsync(data)
    if(result.error){
        return NextResponse.json({
            message: "Error ao validar informações, corrija para prosseguir"
        },{
            status: 400
        })
    }
    const productId = parseInt(Id);
    const product = result.data;
    const response = await updateProductService(productId, product);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json(response.data.message)
}