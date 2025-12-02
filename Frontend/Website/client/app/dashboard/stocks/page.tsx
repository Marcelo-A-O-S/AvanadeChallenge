import StocksClient from "@/components/pages/StocksClient"
import { Metadata } from "next";
export const metadata: Metadata = {
  title: {
    default: "Estoque - Avanade Challenge Dio",
    template: "%s | Estoque - Avanade Challenge Dio",
  },
  description: "Gerenciamento de estoque.",
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
export default function StocksPage(){

    return <StocksClient/>
}