import z from "zod";

export const loginSchema = z.object({
    email: z.email("Gentileza informar um endereço de email válido.").nonempty("Gentileza informe um endereço de email."),
    password: z.string().min(6,"A senha deve ter pelo menos 6 caracteres.")
})
export type LoginSchema = z.infer<typeof loginSchema>