"use client"
import { CartProduct } from "@/types/cart-product";
import { Button } from "../ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog";
import { FormEvent, FormEventHandler, useEffect, useState } from "react";
import { Card, CardAction, CardContent, CardDescription, CardFooter } from "../ui/card";
import { Trash } from "lucide-react";
import { Order } from "@/types/order";
import { Sale } from "@/types/sale";
import { useAuth } from "@/context/auth-context";
import { getUserByEmail } from "@/services/client/userServices";
import { orderSchema } from "@/schemas/OrderSchema";
import { addOrderServices } from "@/services/client/orderServices";
import { toast } from "sonner";
type OrderFormProps = {
    cartProducts: CartProduct[],
    onSuccess: () => Promise<void>
}
export default function OrderForm({ cartProducts, onSuccess }: OrderFormProps) {
    const { user } = useAuth();
    const [open, setOpen] = useState(false);
    const [cartProductsData, setCartProducts] = useState<CartProduct[]>(cartProducts)
    useEffect(() => {
        setCartProducts(cartProducts);
    }, [cartProducts])
    const calculateValues = (cartProducts: CartProduct[]) => {
        let valueTotal = 0;
        cartProducts.map((product) => {
            valueTotal = product.price + valueTotal
        });
        return valueTotal.toFixed(2)
    }
    const onSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!user) {
            return
        }
        const responseUser = await getUserByEmail(user.email);
        if (responseUser.status !== 200 && responseUser.status !== 201) {
            console.log(responseUser.data.message);
            return
        }
        const salesData = cartProductsData.map((cartProduct) => {
            const item: Sale = {
                productId: cartProduct.productId,
                quantity: cartProduct.quantity,
                status: cartProduct.status,
                userId: cartProduct.userId,
                unitPrice: cartProduct.price,
                id: cartProduct.id
            }
            return item
        });
        const order: Order = {
            id: 0,
            sales: salesData,
            status: "PROCESSING",
            userId: responseUser.data.id
        }
        const result = await orderSchema.safeParseAsync(order);
        if (result.error) {
            console.log(result.error.message);
            return
        }
        const response = await addOrderServices(result.data);
        if (response.status !== 200 && response.status !== 201) {
            console.log(response.data.message);
            return toast.error(response.data.message);
        }
        await onSuccess();
        setOpen(false);
        return toast.success("Pedido em processamento...");
    }
    const RemoveProduct = async (cartProduct: CartProduct) => {
        setCartProducts((prev) =>
            prev.filter((p) => p.id !== cartProduct.id)
        );
    }
    const onClear = async () => {
    }
    return (
        <>
            <Dialog
                open={open}
                onOpenChange={(isOpen) => {
                    setOpen(isOpen);
                }}
            >
                <Button asChild className="cursor-pointer">
                    <DialogTrigger >
                    {cartProductsData.length > 0 ? `Finalizar Pedido(${cartProductsData.length})` : `Finalizar Pedido`}
                    </DialogTrigger>
                </Button>
                <DialogContent className="sm:max-w-[425px]">
                    <form onSubmit={onSubmit}>
                        <DialogHeader>
                            <DialogTitle>Dados do Pedido</DialogTitle>
                            <DialogDescription>
                                Preço Total: R$ {calculateValues(cartProductsData)}
                            </DialogDescription>
                        </DialogHeader>
                        <div className="grid grid-cols-1  gap-4 py-1">
                            {cartProducts.map((cartProduct) => (
                                <Card key={cartProduct.id}>
                                    <CardContent>
                                        <CardDescription>Preço: R$ {cartProduct.price.toFixed(2)}</CardDescription>
                                        <CardDescription>Descrição: {cartProduct.description}</CardDescription>
                                        <CardDescription>Quantidade: {cartProduct.quantity} Un</CardDescription>

                                    </CardContent>
                                </Card>
                            ))}
                        </div>
                        <DialogFooter className="py-3">
                            <DialogClose asChild>
                                <Button variant="outline" className="cursor-pointer">Cancel</Button>
                            </DialogClose>
                            <Button type="submit" className="cursor-pointer">Finalizar Pedido</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    )
}