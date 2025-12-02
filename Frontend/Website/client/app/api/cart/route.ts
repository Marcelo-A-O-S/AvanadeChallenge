import { getCartProductsServices } from "@/services/server/cartServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest){
    const searchParams = request.nextUrl.searchParams;
    const pageValue = searchParams.get("page");
    const userIdValue = searchParams.get("userId");
    if(!pageValue && !userIdValue){
        return NextResponse.json({
            message: "Erro ao informar os parametros necessários"
        },{
            status:400
        })
    }
    const page = pageValue? parseInt(pageValue) : 1;
    const userId = userIdValue? parseInt(userIdValue): 1;
    const response = await getCartProductsServices(page,userId);
    if(response.status !== 200 && response.status !== 201){
        return NextResponse.json({
            message:response.data.message
        },{
            status:response.status
        })
    }
    return NextResponse.json(response.data);
}