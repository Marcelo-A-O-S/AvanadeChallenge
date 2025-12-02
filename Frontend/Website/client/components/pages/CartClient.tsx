"use client"
import { useEffect, useState } from "react";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "../ui/pagination";
import { Separator } from "../ui/separator";
import { getCartProductsServices } from "@/services/client/cartServices";
import { useRouter, useSearchParams } from "next/navigation";
import { getUserByEmail } from "@/services/client/userServices";
import { useAuth } from "@/context/auth-context";
import { toast } from "sonner";
import { CartProduct } from "@/types/cart-product";
import { Card, CardAction, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "../ui/card";
import { Button } from "../ui/button";
import ProductView from "../forms/ProductView";
import CartProductForm from "../forms/CartProductForm";
import { deleteSaleServices } from "@/services/client/saleService";
import { Checkbox } from "../ui/checkbox";
import OrderForm from "../forms/OrderForm";

export default function CartClient() {
    const { user } = useAuth();
    const router = useRouter();
    const searchParams = useSearchParams();
    const pageValue = searchParams.get('page');
    const page = pageValue ? parseInt(pageValue) : 1;
    const [cartProducts, setCartProducts] = useState<CartProduct[]>([])
    const [selectProducts, setSelectProducts] = useState<CartProduct[]>([])
    const [loading, setLoading] = useState(false);
    useEffect(() => {
        getCartProducts();
    }, [])
    useEffect(() => {
        getCartProducts();
    }, [page])
    const getCartProducts = async () => {
        setLoading(true);
        try {
            if (user) {
                const responseUser = await getUserByEmail(user.email);
                if (responseUser.status !== 200 && responseUser.status !== 201) {
                    return toast.error(responseUser.data.message);
                }
                const response = await getCartProductsServices(page, responseUser.data.id);
                if (response.status !== 200 && response.status !== 201) {
                    return toast.error(response.data.message);
                }
                const parsed = response.data.map((p:CartProduct[]) => ({
                    ...p,
                    select: false, 
                }));
                console.log(parsed)
                setCartProducts(parsed);
            }
        } finally {
            setLoading(false);
        }
    }
    const onSelect = async (cartProduct: CartProduct, value: boolean) =>{
        if(value){
            setSelectProducts((prev) => [...prev, cartProduct]);
        }else{
            setSelectProducts((prev) =>
                prev.filter((p) => p.id !== cartProduct.id)
            );
        }
    }
    const onSuccess = async () => {
        getCartProducts();
    }
    const onDelete = async (cartProductId: number) => {
        const response = await deleteSaleServices(cartProductId);
        if (response.status !== 200 && response.status !== 201) {
            return toast.error(response.data.message);
        }
        return toast.success(response.data.message);
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
                    <h1 className="text-2xl font-bold">Carrinho de Produtos</h1>
                    <div className="flex justify-end">
                        <OrderForm cartProducts={selectProducts} onSuccess={onSuccess}/>
                    </div>
                    <Separator />
                    <div className="grid grid-cols-1  gap-4 p-5">
                        {cartProducts.map((cartProduct) => (
                            <Card key={cartProduct.id}>
                                <CardHeader>
                                    <CardTitle><Checkbox id="terms-2"  
                                    className="cursor-pointer" 
                                    onCheckedChange={(value) => onSelect(cartProduct, value as boolean)}
                                      />{cartProduct.select} {cartProduct.name}</CardTitle>
                                    <CardAction className="flex justify-center">
                                        <CartProductForm cartProduct={cartProduct} onSuccess={onSuccess} />
                                    </CardAction>
                                </CardHeader>
                                <CardContent>
                                    <CardDescription>Preço: R$ {cartProduct.price.toFixed(2)}</CardDescription>
                                    <CardDescription>Descrição: {cartProduct.description}</CardDescription>
                                    <CardDescription>Quantidade: {cartProduct.quantity} Un</CardDescription>
                                </CardContent>
                                <CardFooter className="flex gap-2">
                                    <ProductView productId={cartProduct.productId} />
                                    <Button variant="destructive" onClick={()=>onDelete(cartProduct.id)} className="cursor-pointer">Cancelar</Button>
                                </CardFooter>
                            </Card>
                        ))}
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
        </>
    )
}