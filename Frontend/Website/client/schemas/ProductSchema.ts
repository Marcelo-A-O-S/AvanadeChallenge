import z from "zod";

export const productSchema = z.object({
    name: z.string().nonempty("Nome é obrigatório."),
    description: z.string().nonempty("A obrigatório pelo menos uma breve descrição."),
    price: z.number({error:"Preço é obrigatório."}).positive("O preço precisa ser maior do que zero.")
        .refine(val => Number(val.toFixed(2)) === val,{
            message: "O preço deve ter no máximo 2 casas decimais."
        }),
    quantity: z.number({ error: "A quantidade é obrigatório."}).int("A quantidade deve ser um número inteiro"),
    minimunStock: z.number({ error: "A quantidade minima de estoque necessita ser informada."}).int("A quantidade deve ser um número inteiro")
})
export type ProductSchema = z.infer<typeof productSchema>