
export type StockMovement = {
    id: number,
    orderId: number | null,
    saleId: number | null,
    productId: number,
    quantity: number,
    type: "Input" | "Output",
    reason: "Purchase"| "Loss"| "Adjustment"| "Sale"|"Reversal",
}