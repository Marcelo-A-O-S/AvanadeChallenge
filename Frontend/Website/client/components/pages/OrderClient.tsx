"use client";
import { useRouter, useSearchParams } from "next/navigation";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "../ui/pagination";
import { Separator } from "../ui/separator";
import { useEffect, useState } from "react";
import { Order } from "@/types/order";
import { DataTable } from "../data-table";
import { useAuth } from "@/context/auth-context";
import { getUserByEmail } from "@/services/client/userServices";
import { getOrdersAllByUserId } from "@/services/client/orderServices";
import { ColumnDef } from "@tanstack/react-table";
import { getOrdersColumns } from "../orders-columns";

export default function OrderClient() {
    const {user} = useAuth()
    const router = useRouter();
    const searchParams = useSearchParams();
    const pageValue = searchParams.get('page');
    const page = pageValue ? parseInt(pageValue) : 1;
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(false);
    const [columns, setColumns] = useState<ColumnDef<Order>[]>([])
    useEffect(() => {
        loadingData();
    }, [])
    const loadingData = async () => {
        setLoading(true)
        try {
            if(!user)
                return;
            const responseUser = await getUserByEmail(user.email);
            if (responseUser.status !== 200 && responseUser.status !== 201) {
                console.log(responseUser.data);
                return;
            }
            const responseOrders = await getOrdersAllByUserId(responseUser.data.id, page);
            if (responseOrders.status !== 200 && responseOrders.status !== 201) {
                console.log(responseOrders.data);
                return;
            }
            console.log(responseOrders.data);
            setColumns(getOrdersColumns)
            setOrders(responseOrders.data);
        } finally {
            setLoading(false)
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
                    <h1 className="text-2xl font-bold">Pedidos</h1>
                    <div className="justify-end">
                    </div>
                    <Separator />
                    <div className="flex justify-center w-full gap-4 p-5">
                        <DataTable columns={columns} data={orders} />
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