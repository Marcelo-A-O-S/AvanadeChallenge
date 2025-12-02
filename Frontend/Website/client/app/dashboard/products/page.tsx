import ProductsClient from "@/components/pages/ProductsClient";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: {
    default: "Products - Avanade Challenge Dio",
    template: "%s | Dashboard - Avanade Challenge Dio",
  },
  description: "Gerenciamento de produtos.",
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
export default function ProductsPage(){
    return <ProductsClient/>
}