import { User } from "@/types/user";
import { ColumnDef } from "@tanstack/react-table";

export const getUsersColumns = (): ColumnDef<User>[]=>{
    const columns: ColumnDef<User>[] = [
        {
            accessorKey: "name",
            header:"Nome do usuário"
        },{
            accessorKey: "email",
            header:"Email"
        }
    ];
    return columns;
}