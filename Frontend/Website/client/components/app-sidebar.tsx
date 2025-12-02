//"use client"
import { Home, Settings, Package, ShoppingBag, Users2, ShoppingCart } from "lucide-react"
import {
    Sidebar,
    SidebarContent,
    SidebarGroup,
    SidebarGroupContent,
    SidebarGroupLabel,
    SidebarHeader,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from "@/components/ui/sidebar"
import { useAuth } from "@/context/auth-context"
import NavUser from "./nav-user"
import { ArrowRightFromLine } from 'lucide-react';
import { ToggleTheme } from "./toggle-theme"
import Link from "next/link"
import { cookies } from "next/headers";
import { verifyToken } from "@/lib/auth";
import ButtonLogout from "./btn-logout";
// Menu items.
const itemsAdmin = [
    {
        title: "Home",
        url: "/dashboard",
        icon: Home,
    },
    {
        title: "Produtos",
        url: "/dashboard/products",
        icon: ShoppingBag,
    },
    {
        title: "Estoque",
        url: "/dashboard/stocks",
        icon: Package,
    }
]
const itemsClient = [
    {
        title: "Home",
        url: "/dashboard",
        icon: Home,
    },
    {
        title: "Catalogo Produtos",
        url: "/dashboard/catalog",
        icon: ShoppingBag,
    },
    {
        title: "Carrinho",
        url: "/dashboard/cart",
        icon: ShoppingCart,
    },
    {
        title: "Pedidos",
        url: "/dashboard/order",
        icon: Package,
    }
]
export async function AppSidebar() {
    const cookieStore = await cookies()
    const token = await cookieStore.get("auth-token")?.value
    if(!token)
        return null;
    const user = await verifyToken(token);
    //const { user, logout } = useAuth();
    return user ? (
        <Sidebar collapsible="icon">
            <SidebarHeader>
                 <NavUser />
             </SidebarHeader>
            <SidebarContent>
                <SidebarGroup>
                    <SidebarGroupLabel>Application</SidebarGroupLabel>
                    <SidebarGroupContent>
                        <SidebarMenu>
                            {user.role == "Administrador" ? itemsAdmin.map((item) => (
                                <SidebarMenuItem key={item.title}>
                                    <SidebarMenuButton asChild>
                                        <Link className="" href={item.url}>
                                            <item.icon />
                                            <span>{item.title}</span>
                                        </Link>
                                    </SidebarMenuButton>
                                </SidebarMenuItem>
                            )):itemsClient.map((item) => (
                                <SidebarMenuItem key={item.title}>
                                    <SidebarMenuButton asChild>
                                        <Link className="" href={item.url}>
                                            <item.icon />
                                            <span>{item.title}</span>
                                        </Link>
                                    </SidebarMenuButton>
                                </SidebarMenuItem>
                            ))}
                        </SidebarMenu>
                    </SidebarGroupContent>
                </SidebarGroup>
            </SidebarContent>
            <SidebarHeader>
                <SidebarMenuItem >
                     <ToggleTheme/>
                 </SidebarMenuItem>
                 <ButtonLogout/>
             </SidebarHeader>
        </Sidebar>
    ) : (
        <></>
    )
}