import { stockMovementSchema } from "@/schemas/StockMovementSchema";
import { addStockMovementService, deleteStockMovementService, getStockPaginationServices, getStockServices } from "@/services/server/stockServices";
import { NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest) {
    const searchParams = request.nextUrl.searchParams
    const page = searchParams.get("page");
    let response = null
    if (page) {
        response = await getStockPaginationServices(parseInt(page));
    } else {
        response = await getStockServices();
    }
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json(response.data)
}
export async function POST(request: NextRequest) {
    try {
        const data = await request.json();
        const result = await stockMovementSchema.safeParseAsync(data);
        if (result.error) {
            return NextResponse.json({
                message: "Erro de válidação."
            }, {
                status: 400
            });
        }
        const stockMovement = await result.data;
        const response = await addStockMovementService(stockMovement);
        if (response.status !== 200 && response.status !== 201) {
            return NextResponse.json({
                message: response.data.message
            }, {
                status: response.status
            })
        }
        return NextResponse.json({
            message: "Movimentação realizada com sucesso!"
        })
    } catch (err) {
        return NextResponse.json({
            message: (err as Error).message
        },{
            status:500
        })
    }
}
export async function DELETE(request: NextRequest) {
    const searchParams = request.nextUrl.searchParams;
    const Id = searchParams.get("Id");
    if (!Id) return NextResponse.json({
        message: "Identificador inválido"
    }, {
        status: 400
    })
    const response = await deleteStockMovementService(parseInt(Id));
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    return NextResponse.json({
        message: response.data.message
    })
}
export async function PUT() {

}