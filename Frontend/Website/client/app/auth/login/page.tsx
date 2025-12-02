import LoginClient from "@/components/pages/LoginClient";
import { Metadata } from "next";
export const metadata: Metadata = {
  title: {
    default: "Login - Avanade Challenge Dio",
    template: "%s | Login - Avanade Challenge Dio",
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
export default function Login() {
    return (
        <>
            <LoginClient/>
        </>)
}