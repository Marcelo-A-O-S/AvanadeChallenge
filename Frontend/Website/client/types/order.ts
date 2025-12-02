import { Sale } from "./sale"

export type Order = {
    id: number,
    userId: number,
    sales: Sale[],
    status: "PROCESSING" | "CONFIRMED" | "REJECT" | "PARTIALLY_CONFIRMED"
}