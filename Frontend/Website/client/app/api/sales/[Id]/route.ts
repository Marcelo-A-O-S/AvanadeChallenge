import { saleSchema } from "@/schemas/SaleSchema";
import { updateSaleServices } from "@/services/server/saleServices";
import { NextRequest, NextResponse } from "next/server";

export async function PUT(request: NextRequest, { params }: { params: Promise<{ Id: string }> }) {
    const { Id } = await params;
    const data = await request.json();
    const result = await saleSchema.safeParseAsync(data);
    if (result.error) {
        return NextResponse.json({
            message: result.error.message
        }, {
            status: 400
        });
    }
    const saleId = parseInt(Id);
    const response = await updateSaleServices(saleId, result.data);
    if (response.status !== 200 && response.status !== 201) {
        return NextResponse.json({
            message: response.data.message
        }, {
            status: response.status
        })
    }
    console.log(response.data);
    return NextResponse.json(response.data.message);
}