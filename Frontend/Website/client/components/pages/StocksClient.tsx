"use client"
import { useEffect, useState } from "react";
import StockForm from "../forms/StockForm";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "../ui/pagination";
import { Separator } from "../ui/separator";
import { StockMovement } from "@/types/stock";
import { deleteStockMovementService, getStockPaginationServices } from "@/services/client/stockServices";
import { toast } from "sonner";
import { DataTable } from "../data-table";
import { ColumnDef } from "@tanstack/react-table";
import SaleView from "../forms/SaleView";
import ProductView from "../forms/ProductView";
import { getStockColumns } from "../stock-columns";
import { useRouter, useSearchParams  } from "next/navigation";
export const columns: ColumnDef<StockMovement>[]= [
        {
            accessorKey: "quantity",
            header: "Quantity"
        },
        {
            accessorKey: "type",
            header: "Movimentação"
        },
        {
            accessorKey: "reason",
            header: "Motivo"
        },
        {
            accessorKey: "saleId",
            header: "Detalhe da venda",
            cell:({getValue}) =>{
                const saleId = getValue() as number;
                if(!saleId) return "Não relacionado a venda";
                return <SaleView saleId={saleId} />
            }
        },
        {
            accessorKey: "orderId",
            header: "Detalhe do Pedido",
            cell:({getValue}) =>{
                const orderId = getValue() as number;
                if(!orderId) return "Não relacionado";
    
            }
    
        },
        {
            accessorKey: "productId",
            header:"Informações do produto",
            cell: ({getValue})=>{
                const productId = getValue() as number;
                return (
                    <ProductView productId={productId}/>
                );  
            }
        },
        {
            accessorKey: "id",
            header:"Ações ",
            cell: ({row})=>{
                const product = row.original;
                return (
                    <>
                    </>
                ) 
            }
        }
    ]

export default function StocksClient() {
    const router = useRouter();
    const searchParams = useSearchParams();
    const pageValue = searchParams.get('page');
    const page = pageValue ? parseInt(pageValue) : 1;
    const [stockMovements, setStockMovements] = useState<StockMovement[]>([]);
    const [isloading, setLoading] = useState(false);
    const [columns, setColumns] = useState<ColumnDef<StockMovement>[]>([])
    useEffect(()=>{
        getStockMovements();
    },[])
    useEffect(()=>{
        getStockMovements();
    },[page])
    const getStockMovements = async() => {
        setLoading(true);
        try{
            const response = await getStockPaginationServices(page)
            if(response.status !== 200 && response.status !== 201){
                return toast.error(response.data.message);
            }
            setColumns(getStockColumns({ onSuccess,onDelete: deleteStockMovement}))
            setStockMovements(response.data);
        }finally{
            setLoading(false);
        }
    }
    const deleteStockMovement = async(id: number) =>{
        setLoading(true);
        try{
            const response = await deleteStockMovementService(id);
            if(response.status !== 200 && response.status !== 201){
                toast.error(response.data.message);
            }
            await onSuccess();
            toast.success(response.data.message);
        }finally{
            setLoading(false);
        }
    }
    const onSuccess = async()=>{
        await getStockMovements();
    }
    const goToPage = (page: number) => {
        const params = new URLSearchParams(searchParams.toString());
        params.set("page", page.toString());
        router.push(`?${params.toString()}`);
    };
    return (
        <>
            <main className="flex min-h-screen w-full flex-col items-center justify-between p-8 bg-white dark:bg-black sm:items-start">
                <section className="w-full flex flex-col gap-4 ">
                    <h1 className="text-2xl font-bold">Gerenciamento de estoque</h1>
                    <div className="justify-end">
                        <StockForm onSuccess={onSuccess} stockMovement={null}/>
                    </div>
                    <Separator />
                    <div className="flex justify-center w-full gap-4 p-5">
                        <DataTable columns={columns} data={stockMovements} />
                    </div>
                    <div>
                        <Pagination>
                            <PaginationContent>
                                <PaginationItem>
                                    <PaginationPrevious href="#"
                                        onClick={(e) => {
                                        e.preventDefault();
                                        goToPage((page || 2) - 1);
                                        }} />
                                </PaginationItem>
                                <PaginationItem>
                                    <PaginationLink href="#"
                                        onClick={(e) => {
                                        e.preventDefault();
                                        goToPage(1);
                                        }}>1</PaginationLink>
                                </PaginationItem>
                                <PaginationItem>
                                    <PaginationEllipsis />
                                </PaginationItem>
                                <PaginationItem>
                                    <PaginationNext
                                        href="#"
                                        onClick={(e) => {
                                        e.preventDefault();
                                        goToPage((page || 1) + 1);
                                        }}
                                    />
                                </PaginationItem>
                            </PaginationContent>
                        </Pagination>
                    </div>
                </section>
            </main>
        </>)
}