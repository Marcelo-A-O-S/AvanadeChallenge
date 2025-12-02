import z from "zod";

export const addCartSchema = z.object({
    quantity:  z.number({ error: "A quantidade é obrigatório."})
    .int("A quantidade deve ser um número inteiro")
    .positive("O preço precisa ser maior do que zero."),
    totalAmount: z.number({error:"Preço é obrigatório."}).positive("O preço precisa ser maior do que zero.")
            .refine(val => Number(val.toFixed(2)) === val,{
                message: "O preço deve ter no máximo 2 casas decimais."
            })
})

export type AddCartSchema = z.infer<typeof addCartSchema>;