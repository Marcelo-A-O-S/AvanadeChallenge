"use client"
import { getSaleByIdService } from "@/services/client/saleService"
import { Sale } from "@/types/sale"
import { useEffect, useState } from "react"
import { toast } from "sonner"
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog"
import { Button } from "../ui/button"

type SaleViewProps = {
    saleId: number
}
export default function SaleView({saleId}:SaleViewProps){
    const [ sale, setSale ] = useState<Sale | null>(null);
    const [ loading, setLoading ] = useState(false);
    useEffect(()=>{
        getSale();
    },[])
    const getSale = async () => {
        setLoading(true);
        try{
            const response = await getSaleByIdService(saleId);
            if(response.status !== 200 && response.status !== 201){
                console.log(response.data);
                return toast.error(response.data.message);
            }
            setSale(response.data);
        }finally{
            setLoading(false);
        }
    }
    return (
        <>
        <Dialog>
            <DialogTrigger>
                <Button className="cursor-pointer">
                    Ver Venda
                </Button>
            </DialogTrigger>
            <DialogContent>
                <DialogHeader>
                <DialogTitle>Detalhes da venda</DialogTitle>
                <DialogDescription>
                    Descrição: 
                </DialogDescription>
                </DialogHeader>
                <DialogFooter className="sm:justify-start">
                    <DialogClose asChild>
                        <Button type="button" variant="secondary">
                            Close
                        </Button>
                    </DialogClose>
                </DialogFooter>
            </DialogContent>
        </Dialog>
        </>
    )
}