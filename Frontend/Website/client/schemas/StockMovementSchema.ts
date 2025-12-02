import z from "zod";
const TypeMovementEnum = z.enum(["Input", "Output"]);
const ReasonMovementEnum = z.enum(["Purchase", "Loss", "Adjustment", "Sale", "Reversal"]);
export const stockMovementSchema = z.object({
    orderId: z.number()
    .min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
    .positive("O Identificador do produto não pode ser negativo.").optional().nullable(),
    saleId:z.number()
    .min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
    .positive("O Identificador do produto não pode ser negativo.").optional().nullable(),
    productId:z.number()
    .min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
    .positive("O Identificador do produto não pode ser negativo."),
    quantity: z.number()
    .min(1,"A quantidade do produto não pode ser igual ou menor a zero.")
    .positive("A quantidade do produto não pode ser negativa."),
    type: TypeMovementEnum,
    reason: ReasonMovementEnum,
})
.refine((data) => !(data.type === "Input" && data.reason === "Loss"), {
    message: "Entrada não pode ter motivo de perda.",
    path: ["reason"],
})
.refine((data) => !(data.type === "Output" && data.reason === "Purchase"), {
    message: "Saída não pode ter motivo de compra.",
    path: ["reason"],
})
.refine((data) => !(data.reason === "Sale" && data.orderId == null), {
    message: "Movimentações de venda precisam de OrderId.",
    path: ["orderId"],
});
export type StockMovementSchema = z.infer<typeof stockMovementSchema>
export type TypeMovement = z.infer<typeof TypeMovementEnum>;
export type ReasonMovement = z.infer<typeof ReasonMovementEnum>;