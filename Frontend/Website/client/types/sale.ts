export type Sale = {
    id?:number,
    userId: number,
    productId: number,
    quantity: number,
    unitPrice:number,
    status: "PROCESSING" | "CONFIRMED" | "REJECT" | "PARTIALLY_CONFIRMED"
}