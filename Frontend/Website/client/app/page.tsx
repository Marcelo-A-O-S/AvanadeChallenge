import { Metadata } from "next";
import Image from "next/image";
import Brasao from "../assets/brasão bootcamp.png"
import { Button } from "@/components/ui/button"
export const metadata: Metadata = {
  title: {
    default: "Avanade Challenge Dio",
    template: "%s | Avanade Challenge Dio",
  },
  description: "Projeto do desafio da Avanade de construção de um sistema usando arquitetura de Micro Serviços para gerenciamento de estoque de produtos e vendas em uma plataforma de e-commerce.",
  authors: [{
    name: 'Marcelo Augusto de Oliveira Soares', url: 'https://github.com/Marcelo-A-O-S'
  }],
  generator: 'Next.js',
  robots: 'index, follow',
  icons: {
    icon: '/favicon.ico',
    shortcut: '/favicon.ico',
  },
  referrer: 'origin-when-cross-origin',
  keywords:
    [
      'avanade',
      'dio',
      'avanade-challenge',
      'avanadechallenge',
    ],
  category: 'education',
  creator: "Marcelo Augusto",
};
export default function Home() {
  return (
    <main className="flex min-h-screen w-full flex-col items-center justify-between py-20 px-16 bg-white dark:bg-black sm:items-start sm:items-start">
      <section className="max-w-7xl mx-auto w-full flex flex-col gap-4 md:flex-row items-center">
        <div className="w-full flex flex-col text-center gap-4 md:w-1/2 lg:text-left">
          <h1 className="text-5xl font-bold bg-linear-to-r from-orange-600 to-pink-500 bg-clip-text text-transparent">Desafio <br/>Técnico Avanade</h1>
          <p className="text-[20px] font-medium">Projeto do desafio da Avanade em parceria com a DIO, de construção de um sistema usando arquitetura de Micro Serviços para gerenciamento de estoque de produtos e vendas em uma plataforma de e-commerce.</p>
          <Button className="text-[20px] font-medium h-10">Iniciar teste</Button>
        </div>
        <div className="w-full flex items-center justify-center md:w-1/2">
          <Image className="w-full h-full" src={Brasao} alt="Brasão bootcamp" width={300} height={300} />
        </div>
      </section>
    </main>
  );
}
