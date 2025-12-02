"use client"
import { Separator } from "../ui/separator";
import ProductForm from "../forms/ProductForm";
import { useEffect, useState } from "react";
import { ProductSchema } from "@/schemas/ProductSchema";
import { deleteProductService, getProductsPagination } from "@/services/client/productServices";
import { Card, CardAction, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "../ui/card";
import { Product } from "@/types/product";
import { toast } from "sonner";
import { Button } from "../ui/button";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "../ui/pagination";
import { useRouter, useSearchParams } from "next/navigation";
export default function ProductsClient() {
    const router = useRouter();
    const searchParams = useSearchParams();
    const pageValue = searchParams.get('page');
    const page = pageValue ? parseInt(pageValue) : 1;
    const [products, setProducts] = useState<Product[]>([])
    const [isloading, setLoading] = useState(false)
    useEffect(() => {
        getProducts()
    }, [])
    useEffect(() => {
        getProducts()
    }, [page])
    const getProducts = async () => {
        setLoading(true);
        try {
            const response = await getProductsPagination(page);
            console.log(response.data);
            if (response.status !== 200 && response.status !== 201) {
                return toast.error(response.data.message);
            }
            setProducts(response.data);
        } finally {
            setLoading(false);
        }
    }
    const deleteProduct = async (id: number) => {
        const response = await deleteProductService(id);
        if (response.status !== 200 && response.status !== 201) {
            return toast.error(response.data.message);
        }
        return toast.success(response.data.message);
    }
    const onSuccess = async () => {
        await getProducts();
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
                    <h1 className="text-2xl font-bold">Gerenciamento de produtos</h1>
                    <div className="justify-end">
                        <ProductForm onSuccess={onSuccess} product={null} />
                    </div>
                    <Separator />
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-5">
                        {products.map((product) => (
                            <Card key={product.id}>
                                <CardHeader>
                                    <CardTitle>{product.name}</CardTitle>
                                    <CardAction>
                                        <p></p>
                                        <p></p>
                                    </CardAction>
                                </CardHeader>
                                <CardContent>
                                    <CardDescription>Preço: R$ {product.price.toFixed(2)}</CardDescription>
                                    <CardDescription>Quantidade: {product.quantity} Unidades</CardDescription>
                                    <CardDescription>{product.description}</CardDescription>
                                </CardContent>
                                <CardFooter className="flex gap-2">
                                    <ProductForm onSuccess={onSuccess} product={product} />
                                    <Button variant="destructive" onClick={() => deleteProduct(product.id)} className="cursor-pointer">Deletar</Button>
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
        </>)
}