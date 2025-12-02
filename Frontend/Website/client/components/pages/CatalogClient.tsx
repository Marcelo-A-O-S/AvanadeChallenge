"use client"
import { useEffect, useState } from "react"
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "../ui/pagination";
import { Separator } from "../ui/separator";
import { getCatalogPaginationServices } from "@/services/client/catalogServices";
import { toast } from "sonner";
import { useRouter, useSearchParams } from "next/navigation";
import { ProductCatalog } from "@/types/product-catalog";
import { Card, CardAction, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "../ui/card";
import { Button } from "../ui/button";
import ProductCatalogForm from "../forms/ProductCatalogForm";

export default function CatalogClient() {
    const router = useRouter();
    const searchParams = useSearchParams();
    const pageValue = searchParams.get('page');
    const page = pageValue ? parseInt(pageValue) : 1;
    const [catalog, setCatalog] = useState<ProductCatalog[]>([]);
    const [loading, setLoading] = useState(false);
    useEffect(() => {
        getCatalog();
    }, [])
    useEffect(() => {
        getCatalog()
    }, [page])
    const getCatalog = async () => {
        setLoading(true);
        try {
            const response = await getCatalogPaginationServices(page);
            if (response.status !== 200 && response.status !== 201) {
                return toast.error(response.data.message);
            }
            console.log(response.data);
            setCatalog(response.data);
        } finally {
            setLoading(false);
        }
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
                    <h1 className="text-2xl font-bold">Catalogo de Produtos</h1>
                    <div className="justify-end">

                    </div>
                    <Separator />
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-5">
                        {catalog.map((product) => (
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
                                    <CardDescription>{product.description}</CardDescription>
                                    <CardDescription>Vendidos: {product.totalSales} Un</CardDescription>
                                </CardContent>
                                <CardFooter className="flex gap-2">
                                    <ProductCatalogForm productCatalog={product}/>
                                    <Button variant="destructive" className="cursor-pointer">Deletar</Button>
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