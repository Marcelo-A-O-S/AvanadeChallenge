import { Order } from "@/types/order";
import { ColumnDef } from "@tanstack/react-table";
import { Button } from "./ui/button";

export const getOrdersColumns = (): ColumnDef<Order>[] =>{
    const columns: ColumnDef<Order>[] = [
        {
            accessorKey: "id",
            header:"ID"
        },
        {
            accessorKey:"createdAt",
            header:"Data de processamento",
            cell:({getValue})=>{
                const createdAt = getValue() as string
                const formated = new Date(createdAt).toLocaleDateString("pt-br");
                return(
                    <>
                    <div>
                        {formated}
                    </div>
                    </>
                )
            }
        },
        {
            accessorKey:"sales",
            header:"Compras",
            cell:() =>{
                return(
                    <Button>
                        Visualizar compras
                    </Button>
                );
            }
        },
        {
            accessorKey:"status",
            header:"Status do Pedido",
            cell:({getValue}) =>{
                const typeStatus = getValue() as string;
                if(typeStatus == "CONFIRMED") return "Processado com sucesso";
                if(typeStatus == "PARTIALLY_CONFIRMED") return "Parcialmente completado";
                return "Saida";
            }
        },{
            header:"Valor total",
            cell:({row})=>{
                const order = row.original;
                let valorTotal = 0;
                order.sales.map((item)=>{
                    valorTotal = (item.quantity * item.unitPrice) + valorTotal;
                })
                return(
                    <div className="font-bold">
                        R$ {valorTotal.toFixed(2)}
                    </div>
                )
            }
        }
    ]
    return columns;
}