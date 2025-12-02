"use client"
import { ArrowRightFromLine } from "lucide-react";
import { SidebarMenuButton, SidebarMenuItem } from "./ui/sidebar";
import { useAuth } from "@/context/auth-context";
export default function ButtonLogout() {
    const { logout } = useAuth();
    return (
        <>
            <SidebarMenuItem >
                <SidebarMenuButton
                    className="cursor-pointer"
                    onClick={logout}
                    asChild>
                    <div >
                        <ArrowRightFromLine />
                        <span>Log Out</span>
                    </div>
                </SidebarMenuButton>
            </SidebarMenuItem>
        </>
    )
}