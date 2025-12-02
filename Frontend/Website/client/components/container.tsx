import { ReactNode } from "react"
import { ThemeProvider } from "./theme-provider"
import Header from "./header"
import Footer from "./footer"
import { AuthProvider } from "@/context/auth-context"
import { SidebarProvider, SidebarTrigger } from "./ui/sidebar"
import { AppSidebar } from "./app-sidebar"
import { Toaster } from "@/components/ui/sonner";
import { cookies } from "next/headers"
import { verifyToken } from "@/lib/auth"
import { User } from "@/types/user"
type ContainerProps = {
    children: ReactNode
}
export default function Container({ children }: ContainerProps) {
    return (
        <>
            <ThemeProvider
                attribute="class"
                defaultTheme={"system"}
                enableSystem
                disableTransitionOnChange

            >
                <AuthProvider >
                    <SidebarProvider >
                        <Header />
                        <AppSidebar />
                        <SidebarTrigger className="" />
                        <Toaster />
                        {children}
                        <Footer />
                    </SidebarProvider>
                </AuthProvider>
            </ThemeProvider>
        </>
    )
}