import Image from "next/image"
import { Menu } from 'lucide-react';
import { House } from "lucide-react";
import {
    Sheet,
    SheetClose,
    SheetContent,
    SheetTrigger,
} from "@/components/ui/sheet"
import { ToggleTheme } from "./toggle-theme";
import Link from "next/link"
import { Button, buttonVariants } from "./ui/button";
import { cookies } from "next/headers";
import { verifyToken } from "@/lib/auth";
import { removeAuthToken } from "@/actions/remove-token";
export default async function Header() {
    const cookieStore = await cookies()
    const token = await cookieStore.get("auth-token")?.value;
    if (token){
        const user = await verifyToken(token);
        if(user){
            return null;
        }else{
            removeAuthToken();
        }
    }
    return (
        <>
            <nav className=" w-full flex px-8 h-16 md:h-18 lg:h-20 border-b shadow-[0_4px_6px_rgba(0,0,0,0.1)] fixed top-0 left-0 bg-white dark:bg-black">
                <div className="max-w-7xl mx-auto flex gap-1 w-full justify-between items-center h-full">
                    <Link href={"/"} className="flex items-center gap-1">
                        <Image src="https://digitalassets.avanade.com/api/public/content/favicon?v=ff268e09" alt="" width={30} height={30} />
                        <span className="text-2xl font-bold bg-linear-to-r from-orange-600 to-pink-500 bg-clip-text text-transparent">Avanade Challenge</span>
                    </Link>
                    <div className="flex gap-4 items-center">
                        <ul className="hidden md:flex items-center gap-3">

                            <li><Link className="flex gap-2" href={"/"}><House />Home</Link></li>
                            <li><Link className={buttonVariants({ variant: "default" })} href="/auth/login">Acessar Projeto</Link></li>
                            <li><Link className={buttonVariants({ variant: "default" })} href="/auth/register">Realizar registro</Link></li>
                        </ul>
                        <ToggleTheme />
                        <Sheet>
                            <SheetTrigger asChild><Menu className="md:hidden" /></SheetTrigger>
                            <SheetContent className="py-20 px-4 w-full">
                                <SheetClose asChild className="w-full"><Link className="flex gap-2 w-full" href={"/"}><House />Home</Link></SheetClose>
                                <SheetClose asChild className="w-full"><Link className={buttonVariants({ variant: "default" }) + ` w-full`} href="/auth/login">Acessar Projeto</Link></SheetClose>
                                <SheetClose asChild ><Link className={buttonVariants({ variant: "default" }) + ` w-full`} href="/auth/register">Realizar registro</Link></SheetClose>
                            </SheetContent>
                        </Sheet>
                    </div>
                </div>
            </nav>
        </>)
    
}