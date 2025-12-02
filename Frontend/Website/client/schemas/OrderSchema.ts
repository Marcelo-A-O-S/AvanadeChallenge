import z from "zod";
import { saleSchema } from "./SaleSchema";
export const OrderStatusEnum = z.enum(["PROCESSING", "CONFIRMED", "REJECT", "PARTIALLY_CONFIRMED"]);
export const orderSchema = z.object({
    id: z.number("O identificador é obrigatório!")
        .optional(),
    status: OrderStatusEnum,
    userId: z.number("O identificador é obrigatório!").min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
        .positive("O Identificador do produto não pode ser negativo."),
    sales: z.array(saleSchema)
        .min(1,"O obrigatório informar pelo menos 1 compra ao pedido!")
})

export type OrderSchema = z.infer<typeof orderSchema>