import { Metadata } from "next";
import RegisterClient from "@/components/pages/RegisterClient";
export const metadata: Metadata = {
  title: {
    default: "Registrar - Avanade Challenge Dio",
    template: "%s | Registrar - Avanade Challenge Dio",
  },
  description: "Área de acesso do sistema.",
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
export default function Register(){
    return <RegisterClient/>
}