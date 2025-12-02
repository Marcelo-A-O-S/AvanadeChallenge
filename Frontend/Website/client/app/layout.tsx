import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import { SegoeUi } from "@/fonts";
import Container from "@/components/container";
const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="pt-br">
      <body
        className={`${SegoeUi.className} m-0  bg-white dark:bg-black antialiased`}
      >
        <Container>
        {children}
        </Container>
      </body>
    </html>
  );
}
