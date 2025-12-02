export type CartProduct = {
    id: number,
    userId:number,
    productId: number,
    name: string,
    description: string,
    quantity: number,
    price: number,
    status: "PROCESSING" | "CONFIRMED" | "REJECT" | "PARTIALLY_CONFIRMED",
    select: boolean 
}