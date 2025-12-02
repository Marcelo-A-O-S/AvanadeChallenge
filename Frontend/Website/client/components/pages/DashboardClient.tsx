"use client"

import { useAuth } from "@/context/auth-context"
import {
    Empty,
    EmptyDescription,
    EmptyHeader,
    EmptyTitle,
} from "@/components/ui/empty"
import { useEffect, useState } from "react"
import { getProductQuantity } from "@/services/client/productServices"
import { getUserByEmail, getUsersQuantity } from "@/services/client/userServices"
import { getStockMovementQuantity } from "@/services/client/stockServices"
import { email } from "zod"
import { getSalesQuantityByUserId } from "@/services/client/saleService"
import { getOrderQuantityByUserId } from "@/services/client/orderServices"

export default function DashboadClient() {
    const { user } = useAuth()
    const [ countProducts, setCountProducts ] = useState(0);
    const [ countUsers, setCountUsers ] = useState(0);
    const [ countMovements, setCountMovements ] = useState(0);
    const [ countSales, setCountSales ] = useState(0);
    const [ countOrders, setCountOrders ] = useState(0);
    const [ loading, setLoading ] = useState(false);
    useEffect(()=>{
        loadingData();
    },[])
    const loadingData = async () =>{
        setLoading(true);
        try{
            if(!user)
                return
            if(user.role == "Administrador"){
                const responseProducts = await getProductQuantity();
                if(responseProducts.status !== 200 && responseProducts.status !== 201){
                    console.log(responseProducts.data);
                    return;
                }
                setCountProducts(responseProducts.data);
                const responseUsers = await getUsersQuantity();
                if(responseUsers.status !== 200 && responseUsers.status !== 201){
                    console.log(responseUsers.data);
                    return;
                }
                console.log("Users: ",responseUsers.data);
                setCountUsers(responseUsers.data);
                const responseMovements = await getStockMovementQuantity();
                if(responseMovements.status !== 200 && responseMovements.status !== 201){
                    console.log(responseMovements.data);
                    return;
                }
                setCountMovements(responseMovements.data);
            }else{
                const responseUser = await getUserByEmail(user.email);
                if(responseUser.status !== 200 && responseUser.status !== 201){
                    console.log(responseUser.data);
                    return;
                }
                console.log(responseUser.data);
                const responseSale = await getSalesQuantityByUserId(responseUser.data.id);
                if(responseSale.status !== 200 && responseSale.status !== 201){
                    console.log(responseSale.data);
                    return;
                }
                setCountSales(responseSale.data);
                const responseOrders = await getOrderQuantityByUserId(responseUser.data.id);
                if(responseOrders.status !== 200 && responseOrders.status !== 201){
                    console.log(responseOrders.data);
                    return;
                }
                setCountOrders(responseOrders.data);
            }
        }finally{

        } 
    }
    return user?.role == "Administrador" ? (
        <div className="flex flex-1 flex-col gap-6 p-4">
            <div className="grid gap-4 md:grid-cols-3">
                <div className="bg-muted/50 p-6 rounded-xl flex flex-col justify-between shadow-sm">
                    <p className="text-sm text-muted-foreground">Usuários cadastrados</p>
                    <h2 className="text-3xl font-bold mt-2">{countUsers}</h2>
                </div>

                <div className="bg-muted/50 p-6 rounded-xl flex flex-col justify-between shadow-sm">
                    <p className="text-sm text-muted-foreground">Produtos Ativos</p>
                    <h2 className="text-3xl font-bold mt-2">{countProducts}</h2>
                </div>

                <div className="bg-muted/50 p-6 rounded-xl flex flex-col justify-between shadow-sm">
                    <p className="text-sm text-muted-foreground">Movimentações</p>
                    <h2 className="text-3xl font-bold mt-2">{countMovements}</h2>
                </div>
            </div>
            <div className="bg-muted/50 rounded-xl p-10 shadow-sm flex items-center justify-center">
                <Empty>
                    <EmptyHeader>
                        <EmptyTitle>
                            Olá, seja bem-vindo{" "}
                            <span className="font-bold">{user?.name}</span>!
                        </EmptyTitle>
                        <EmptyDescription>
                            Aqui você poderá gerenciar usuários, monitorar atividades
                            e acompanhar as métricas do seu sistema.
                        </EmptyDescription>
                    </EmptyHeader>
                </Empty>
            </div>
        </div>
    ) : (
        <div className="flex flex-1 flex-col gap-6 p-4">
            <div className="grid gap-4 md:grid-cols-3">
                <div className="bg-muted/50 p-6 rounded-xl flex flex-col justify-between shadow-sm">
                    <p className="text-sm text-muted-foreground">Meus pedidos</p>
                    <h2 className="text-3xl font-bold mt-2">{countOrders}</h2>
                </div>

                <div className="bg-muted/50 p-6 rounded-xl flex flex-col justify-between shadow-sm">
                    <p className="text-sm text-muted-foreground">Compras em andamento</p>
                    <h2 className="text-3xl font-bold mt-2">{countSales}</h2>
                </div>

                <div className="bg-muted/50 p-6 rounded-xl flex flex-col justify-between shadow-sm">
                    <p className="text-sm text-muted-foreground">Pontos / Benefícios</p>
                    <h2 className="text-3xl font-bold mt-2">0</h2>
                </div>
            </div>
            <div className="bg-muted/50 rounded-xl p-10 shadow-sm flex items-center justify-center">
                <Empty>
                    <EmptyHeader>
                        <EmptyTitle>
                            Olá, <span className="font-bold">{user?.name}</span> 👋
                        </EmptyTitle>
                        <EmptyDescription>
                            Aqui você pode acompanhar o status dos seus pedidos, visualizar seu histórico
                            e consultar seus benefícios de cliente.
                        </EmptyDescription>
                    </EmptyHeader>
                </Empty>
            </div>
        </div>
    )
}
