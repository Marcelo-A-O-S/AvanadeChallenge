import LoginForm from "../forms/LoginForm";
import { Card, CardContent } from "../ui/card";

export default function LoginClient() {
    return (
        <>
            <main className="py-10">
                <section className="flex min-h-screen w-full flex-col items-center justify-center" id="formulario">
                    <Card className="w-3/4 h-full md:h-[500px]">
                        <CardContent className="flex flex-col h-full items-stretch md:gap-5 md:flex-row-reverse">
                            <div className="flex flex-col p-8 bg-linear-to-r from-orange-600 to-pink-500 s md:w-1/2 rounded-2xl">
                                <h1 className="text-2xl font-bold text-white">Etapa de login</h1>
                                <p className="text-white leading-relaxed">Nesta etapa, você poderá acessar sua conta para usar às funcionalidades do sistema,
                                    explorando seus recursos construidos envolvendo os problemas propostos pelo bootcamp da DIO em parceria com a Avanade.</p>
                            </div>
                            <div className="flex flex-col justify-center gap-5 p-4 md:w-1/2 md:p-8">
                                <div className="flex flex-col gap-2">
                                    <h1 className="text-2xl font-bold bg-linear-to-r from-orange-600 to-pink-500 bg-clip-text text-transparent">Login</h1>
                                    <p>Informe os dados necessários para prosseguir com o acesso a sua conta:</p>
                                </div>
                                <LoginForm />
                            </div>
                        </CardContent>
                    </Card>
                </section>
            </main>
        </>
    )
}