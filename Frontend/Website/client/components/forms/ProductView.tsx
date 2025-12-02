import { Product } from "@/types/product"
import { useEffect, useState } from "react"
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog";
import { getProductByIdService } from "@/services/client/productServices";
import { toast } from "sonner";
import { Button } from "../ui/button";
type ProductViewProps = {
    productId: number
}
export default function ProductView({ productId }: ProductViewProps){
    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(false);
    useEffect(()=>{
        getProduct()
    },[])
    const getProduct = async () =>{
        setLoading(true);
        try{
            const response = await getProductByIdService(productId)
            if(response.status !== 200 && response.status !== 201){
                console.log(response.data.message);
                return toast.error(response.data.message);
            }
            console.log(response.data);
            setProduct(response.data);
        }finally{
            setLoading(false);
        }
    }
    return(
        <>
        <Dialog>
            <DialogTrigger>
                <Button className="cursor-pointer">
                    Detalhes do produto
                </Button>
            </DialogTrigger>
            <DialogContent>
                <DialogHeader>
                <DialogTitle>{product? product.name : ""}</DialogTitle>
                <DialogDescription>
                    Descrição: {product? product.description: ""}
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