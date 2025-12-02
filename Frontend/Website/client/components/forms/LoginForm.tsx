'use client'
import { loginSchema, LoginSchema } from "@/schemas/LoginSchema"
import { zodResolver } from "@hookform/resolvers/zod"
import { Controller, useForm } from "react-hook-form"
import { InputGroup, InputGroupAddon, InputGroupInput } from "../ui/input-group"
import { LockKeyholeOpen, MailIcon } from "lucide-react"
import { Button } from "../ui/button"
import { useAuth } from "@/context/auth-context"
import { useRouter } from "next/navigation"
import { toast } from "sonner"
export default function LoginForm() {
    const router = useRouter();
    const auth = useAuth();
    const { control, handleSubmit, formState: { errors } } = useForm<LoginSchema>({
        resolver: zodResolver(loginSchema),
        defaultValues: {
            email: '',
            password: ''
        }
    });
    const onSubmit = async (data: LoginSchema) => {
        const response = await auth.login(data.email, data.password);
        if(response.status == 200){
            toast.success("Seja bem vindo!");
            router.push("/dashboard/");
            return router.refresh();
        }
        return toast.error(response.data.message);
    };
    return (
        <>
            <form className="flex flex-col gap-5" onSubmit={handleSubmit(onSubmit)}>
                <Controller
                    name="email"
                    control={control}
                    render={({ field }) => (
                        <div>
                            <InputGroup>
                                <InputGroupInput {...field} placeholder="Informe seu email" />
                                <InputGroupAddon>
                                    <MailIcon />
                                </InputGroupAddon>
                            </InputGroup>
                            {errors.email && <p className="text-red-500">{errors.email.message}</p>}
                        </div>
                    )}
                />
                <Controller
                    name="password"
                    control={control}
                    render={({ field }) => (
                        <div>
                            <InputGroup>
                                <InputGroupInput {...field} type="password" placeholder="Informe sua senha" />
                                <InputGroupAddon>
                                    <LockKeyholeOpen />
                                </InputGroupAddon>
                            </InputGroup>
                            {errors.password && <p className="text-red-500">{errors.password.message}</p>}
                        </div>
                    )}
                />
                <Button className="cursor-pointer" type="submit">Acessar</Button>
            </form>
        </>
    )
}