import { Order } from "@/types/order";
import { ColumnDef } from "@tanstack/react-table";

export const getOrdersColumns = (): ColumnDef<Order>[] =>{
    const columns: ColumnDef<Order>[] = [
        {
            accessorKey: ""
        }
    ]
    return columns;
}