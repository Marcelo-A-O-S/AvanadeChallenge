import { orderSchema } from "@/schemas/OrderSchema";
import { addOrderServices, getOrdersAllByUserId, getOrdersByUserId, getOrdersServices } from "@/services/server/orderServices";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
    const data = await request.json();
    const result = await orderSchema.safeParseAsync(data);
    if (result.error) {
        return NextResponse.json({
            message: result.error.message
        }, {
            status: 400
        })
    }
    const response = await addOrderServices(result.data);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json({ message: "O Pedido está em processamento..." })
}
export async function GET(request: NextRequest) {
    const searchParams = request.nextUrl.searchParams;
    const userIdValue = searchParams.get("userId");
    const pageValue = searchParams.get("page");
    if (!userIdValue && !pageValue) {
        return NextResponse.json({
            message: "Gentileza informar os dados necessários"
        },{
            status: 400
        })
    }
    const userId = userIdValue ? parseInt(userIdValue) : 1;
    const page = pageValue ? parseInt(pageValue) : 1;
    const response = await getOrdersAllByUserId(userId, page);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json(response.data);
}