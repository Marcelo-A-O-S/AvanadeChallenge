"use client"
import { registerSchema, type RegisterSchema } from "@/schemas/RegisterSchema"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm, Controller } from "react-hook-form"
import { InputGroup, InputGroupAddon, InputGroupInput } from "../ui/input-group"
import { MailIcon } from "lucide-react"
import { CircleUserRound } from 'lucide-react';
import { LockKeyholeOpen } from 'lucide-react';
import { LockKeyhole } from 'lucide-react';
import { Button } from "../ui/button"
import { useAuth } from "@/context/auth-context"
import { useEffect } from "react"
import { toast } from "sonner"
export default function RegisterForm() {
    const auth = useAuth();
    useEffect(()=>{
        
    },[])
    const { control, handleSubmit, formState: { errors } } = useForm<RegisterSchema>({
        resolver: zodResolver(registerSchema),
        defaultValues: {
            name: '',
            email: '',
            password: '',
            passwordConfirm: ''
        }
    })
    const onSubmit = async (data: RegisterSchema) => {
        const response = await auth.register(data.name,data.email,data.password,data.passwordConfirm);
        console.log(response);
        if(response.status == 200){
            return toast.success(response.data.message);
        }
        return toast.error(response.data.message);;
    }
    return (
        <>
            <form className="flex flex-col gap-5" onSubmit={handleSubmit(onSubmit)}>
                <Controller
                    name="name"
                    control={control}
                    render={({ field, fieldState }) => (
                        <div>
                            <InputGroup>
                                <InputGroupInput {...field} placeholder="Informe seu nome" />
                                <InputGroupAddon>
                                    <CircleUserRound />
                                </InputGroupAddon>
                            </InputGroup>
                            {errors.name && <p className="text-red-500">{errors.name.message}</p>}
                        </div>
                    )}
                />
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
                <Controller
                    name="passwordConfirm"
                    control={control}
                    render={({ field }) => (
                        <div>
                            <InputGroup>
                                <InputGroupInput {...field} type="password" placeholder="Confirme sua senha" />
                                <InputGroupAddon>
                                    <LockKeyhole />
                                </InputGroupAddon>
                            </InputGroup>
                            {errors.passwordConfirm && <p className="text-red-500">{errors.passwordConfirm.message}</p>}
                        </div>
                    )}
                />

                <Button className="cursor-pointer" type="submit">Registrar</Button>
            </form>
        </>
    )
}