import DashboadClient from "@/components/pages/DashboardClient";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: {
    default: "Dashboard - Avanade Challenge Dio",
    template: "%s | Dashboard - Avanade Challenge Dio",
  },
  description: "Dashboard de gerenciamento.",
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
export default function DashboadPage(){
    return <DashboadClient/>
}