import z from "zod";

export const registerSchema = z.object({
    name: z.string("Gentileza informar um nome válido").nonempty("Gentileza informar um nome para cadastro."),
    email: z.email("Gentileza informar um endereço de email válido.").nonempty("Gentileza informe um endereço de email."),
    password: z.string().min(6, "A senha deve ter pelo menos 6 caracteres.").nonempty("Gentileza informar a senha de acesso para cadastro."),
    passwordConfirm: z.string().nonempty("Gentileza confirmar sua senha.")
}).refine((data) => data.password == data.passwordConfirm, {
    message:"As senhas não coincidem.",
    path:["passwordConfirm"]
})
export type RegisterSchema = z.infer<typeof registerSchema>
