"use client"
import { SidebarMenu, SidebarMenuButton, SidebarMenuItem } from "./ui/sidebar";
import { Avatar } from "@radix-ui/react-avatar";
import { useAuth } from "@/context/auth-context";
import { AvatarFallback } from "./ui/avatar";
import { User } from "@/types/user";

export default function NavUser() {
    const { user } = useAuth();
    if (!user) return null;
    return (
        <SidebarMenu>
            <SidebarMenuItem>
                <SidebarMenuButton
                    size="lg"
                    className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
                >
                    <Avatar className="h-8 w-8 rounded-lg grayscale">
                        <AvatarFallback className="rounded-lg">{user?.name?.slice(0, 2)}</AvatarFallback>
                    </Avatar>
                    <div className="grid flex-1 text-left text-sm leading-tight">
                        <span className="truncate font-medium">{user.name}</span>
                        <span className="text-muted-foreground truncate text-xs">
                            {user.email}
                        </span>
                    </div>
                </SidebarMenuButton>
            </SidebarMenuItem>
        </SidebarMenu>
    );
}