import RegisterForm from "../forms/RegisterForm"
import { Card, CardAction, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "../ui/card"
export default function RegisterClient() {

    return (
        <>
            <main className="">
                <section className="flex min-h-screen w-full h-full flex-col items-center justify-center py-20" id="formulario">
                    <Card className="w-3/4 h-full md:h-[500px]">
                        <CardContent className="flex flex-col h-full items-stretch md:gap-5 md:flex-row">
                            <div className="flex flex-col p-8 bg-linear-to-r from-orange-600 to-pink-500 s md:w-1/2 rounded-2xl">
                                <h1 className="text-2xl font-bold text-white">Etapa de registro</h1>
                                <p className="text-white leading-relaxed">Nesta etapa, você poderá criar sua conta para ter acesso às funcionalidades do sistema,
                                possibilitando explorar sua funcionalidade e após isso, outros recursos construidos envolvendo os problemas propostos pelo bootcamp da DIO em parceria com a Avanade.</p>
                            </div>
                            <div className="flex flex-col justify-center gap-5 p-4 md:w-1/2 md:p-8">
                                <div className="flex flex-col gap-2">
                                    <h1 className="text-2xl font-bold bg-linear-to-r from-orange-600 to-pink-500 bg-clip-text text-transparent">Registro</h1>
                                    <p>Informe os dados necessários para prosseguir com o registro da sua conta:</p>
                                </div>
                                <RegisterForm />
                            </div>
                        </CardContent>
                    </Card>
                </section>
            </main>
        </>
    )
}