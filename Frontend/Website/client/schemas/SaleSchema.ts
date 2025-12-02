import z from "zod";
export const SalesStatusEnum = z.enum(["PROCESSING", "CONFIRMED", "REJECT", "PARTIALLY_CONFIRMED"]);
export const saleSchema = z.object({
    id: z.number()
        .min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
        .positive("O Identificador do produto não pode ser negativo.").optional(),
    userId: z.number()
        .min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
        .positive("O Identificador do produto não pode ser negativo."),
    productId: z.number()
        .min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
        .positive("O Identificador do produto não pode ser negativo."),
    quantity: z.number()
        .min(1,"O Identificador do produto não pode ser igual ou menor a zero.")
        .positive("O Identificador do produto não pode ser negativo."),
    unitPrice: z.number({error:"Preço é obrigatório."}).positive("O preço precisa ser maior do que zero.")
            .refine(val => Number(val.toFixed(2)) === val,{
                message: "O preço deve ter no máximo 2 casas decimais."
            }),
    status: SalesStatusEnum
});
export type SaleSchema = z.infer<typeof saleSchema>;