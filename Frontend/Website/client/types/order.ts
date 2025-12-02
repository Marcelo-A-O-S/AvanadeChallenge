import { Sale } from "./sale"

export type Order = {
    id: number,
    userId: number,
    sales: Sale[],
    createdAt?: Date,
    status: "PROCESSING" | "CONFIRMED" | "REJECT" | "PARTIALLY_CONFIRMED"
}