import { ColumnDef } from "@tanstack/react-table";
import ProductView from "./forms/ProductView";
import { StockMovement } from "@/types/stock";
import SaleView from "./forms/SaleView";
import StockForm from "./forms/StockForm";
import { Button } from "./ui/button";

type StockColumnsProps = {
    onSuccess: () => Promise<void>;
    onDelete: (id: number) => Promise<void>
}
export const getStockColumns = ({ onSuccess, onDelete }: StockColumnsProps): ColumnDef<StockMovement>[] => {
    const columns: ColumnDef<StockMovement>[] = [
        {
            accessorKey: "quantity",
            header: "Quantidade"
        },
        {
            accessorKey: "type",
            header: "Movimentação",
            cell:({getValue}) =>{
                const typeMovement = getValue() as string;
                if(typeMovement == "Input") return "Entrada";
                return "Saida";
            }
        },
        {
            accessorKey: "reason",
            header: "Motivo",
        },
        {
            accessorKey: "saleId",
            header: "Detalhe da venda",
            cell: ({ getValue }) => {
                const saleId = getValue() as number;
                if (!saleId) return "Não relacionado a venda";
                return <SaleView saleId={saleId} />
            }
        },
        {
            accessorKey: "orderId",
            header: "Detalhe do Pedido",
            cell: ({ getValue }) => {
                const orderId = getValue() as number;
                if (!orderId) return "Não relacionado";

            }

        },
        {
            accessorKey: "productId",
            header: "Informações do produto",
            cell: ({ getValue }) => {
                const productId = getValue() as number;
                return (
                    <ProductView productId={productId} />
                );
            }
        },
        {
            accessorKey: "id",
            header: "Ações ",
            cell: ({ row }) => {
                const product = row.original;
                return (
                    <>
                        <div className="flex gap-2">
                            <StockForm onSuccess={onSuccess} stockMovement={product} />
                            <Button variant={"outline"} className="cursor-pointer" onClick={() => onDelete(product.id)}>Deletar</Button>
                        </div>
                    </>
                )
            }
        }
    ]
    return columns;
} 