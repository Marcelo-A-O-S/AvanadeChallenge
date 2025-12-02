"use client";
import { stockMovementSchema, StockMovementSchema } from "@/schemas/StockMovementSchema";
import { StockMovement } from "@/types/stock";
import { zodResolver } from "@hookform/resolvers/zod";
import { Controller, useForm } from "react-hook-form";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog";
import { Button } from "../ui/button";
import { InputGroup, InputGroupAddon, InputGroupInput, InputGroupText, InputGroupTextarea } from "../ui/input-group";
import { Boxes } from "lucide-react";
import { useEffect, useState } from "react";
import { Product } from "@/types/product";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "../ui/select";
import { getProducts, updateProductService } from "@/services/client/productServices";
import { toast } from "sonner";
import { Checkbox } from "../ui/checkbox";
import { Label } from "../ui/label";
import { addStockMovimentService, updateStockMovementService } from "@/services/client/stockServices";
type StockFormProps = {
    stockMovement: StockMovement | null,
    onSuccess: () => Promise<void>
};
export default function StockForm({ stockMovement, onSuccess }: StockFormProps) {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(false);
    const { control, handleSubmit, formState: { errors }, setValue } = useForm<StockMovementSchema>({
        resolver: zodResolver(stockMovementSchema),
        defaultValues: {
            orderId: null,
            saleId: null,
            productId: 0,
            quantity: 0,
            type: "Input",
            reason: "Adjustment"
        }
    });
    useEffect(() => {
        if(stockMovement != null){
            onLoadingForm(stockMovement);
        }
        onLoadingProducts();
    }, [])
    const onLoadingForm = async(stockMovement: StockMovement)=>{
        setLoading(true);
        try{
            setValue("orderId", stockMovement.orderId);
            setValue("productId", stockMovement.productId);
            setValue("saleId",stockMovement.saleId);
            setValue("reason",stockMovement.reason);
            setValue("quantity",stockMovement.quantity);
            setValue("type",stockMovement.type);
        }finally{
            setLoading(false);
        }
    }
    const onSubmit = async (data: StockMovementSchema) => {
        let response = null;
        if(stockMovement){
            response = await updateStockMovementService(stockMovement.id,data);
            if (response.status !== 200 && response.status !== 201) {
                console.log(response.data);
                return toast.error("Erro ao salvar movimento:", response.data.message);
            }
            await onSuccess();
            return toast.success(response.data.message);
        }else{
            response = await addStockMovimentService(data);
            if (response.status !== 200 && response.status !== 201) {
                console.log(response.data);
                return toast.error("Erro ao salvar movimento:", response.data.message);
            }
            await onSuccess();
            return toast.success(response.data.message);
        }
    }
    const onLoadingProducts = async () => {
        try {
            setLoading(true);
            const response = await getProducts();
            if (response.status !== 200 && response.status !== 201) {
                return toast.error(response.data.message);
            }
            setProducts(response.data)
        } finally {
            setLoading(false);
        }
    }
    return (
        <>
            <Dialog>
                <DialogTrigger asChild>
                    <Button className="cursor-pointer">{stockMovement ? "Editar " : "Movimentar estoque"}</Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader>
                            <DialogTitle>{stockMovement ? "Atualizar movimento" : "Criar movimento"}</DialogTitle>
                            <DialogDescription>
                                Informe os dados abaixo:
                            </DialogDescription>
                        </DialogHeader>
                        <div className="grid gap-4">
                            <div className="grid gap-3">
                                <Controller
                                    name="orderId"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Pedido:
                                            </DialogDescription>
                                            <InputGroup
                                                className="cursor-not-allowed"
                                            >
                                                <InputGroupInput  {...field}
                                                    value={field.value ?? ""}
                                                    type="number"
                                                    disabled
                                                    onChange={e => field.onChange(e.target.value ? null : parseInt(e.target.value))}
                                                    placeholder="Informe o identificador do pedido" />
                                                <InputGroupAddon>
                                                    <Boxes />
                                                </InputGroupAddon>
                                            </InputGroup>
                                            {errors.orderId && <p className="text-red-500">{errors.orderId.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div className="grid gap-3">
                                <Controller
                                    name="saleId"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Venda:
                                            </DialogDescription>
                                            <InputGroup
                                                className="cursor-not-allowed"
                                            >
                                                <InputGroupInput  {...field}
                                                    value={field.value ?? ""}
                                                    type="number"
                                                    disabled

                                                    onChange={e => field.onChange(e.target.value ? null : parseInt(e.target.value))}
                                                    placeholder="Informe um identificador da venda" />
                                                <InputGroupAddon>
                                                    <Boxes />
                                                </InputGroupAddon>
                                            </InputGroup>
                                            {errors.saleId && <p className="text-red-500">{errors.saleId.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div className="grid gap-3">
                                <Controller
                                    name="productId"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Produto:
                                            </DialogDescription>
                                            <Select
                                                onValueChange={(value) => field.onChange(Number(value))}
                                                value={String(field.value)}

                                            >
                                                <SelectTrigger className="w-full">
                                                    <SelectValue placeholder="Selecione o produto" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    <SelectGroup>
                                                        <SelectLabel>Produtos</SelectLabel>
                                                        {products.map((product) => (
                                                            <SelectItem key={product.id} value={String(product.id)}>{product.name}</SelectItem>
                                                        ))}
                                                    </SelectGroup>
                                                </SelectContent>
                                            </Select>
                                            {errors.productId && <p className="text-red-500">{errors.productId.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div className="grid gap-3">
                                <Controller
                                    name="quantity"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Quantidade:
                                            </DialogDescription>
                                            <InputGroup>
                                                <InputGroupInput  {...field}
                                                    type="number"
                                                    onChange={e => field.onChange(parseInt(e.target.value))}
                                                    placeholder="Informe uma quantidade para o produto" />
                                                <InputGroupAddon>
                                                    <Boxes />
                                                </InputGroupAddon>
                                            </InputGroup>
                                            {errors.quantity && <p className="text-red-500">{errors.quantity.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div className="grid gap-3">
                                <Controller
                                    name="type"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Tipo de movimentação:
                                            </DialogDescription>
                                            <Select
                                                onValueChange={field.onChange}
                                                value={field.value}
                                            >
                                                <SelectTrigger className="w-full">
                                                    <SelectValue placeholder="Selecione o tipo de movimentação" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    <SelectGroup>
                                                        <SelectLabel>Movimentações</SelectLabel>
                                                        <SelectItem value="Input">Entrada</SelectItem>
                                                        <SelectItem value="Output">Saída</SelectItem>
                                                    </SelectGroup>
                                                </SelectContent>
                                            </Select>
                                            {errors.type && <p className="text-red-500">{errors.type.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div className="grid gap-3">
                                <Controller
                                    name="reason"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Motivo do movimento:
                                            </DialogDescription>
                                            <Select
                                                onValueChange={field.onChange}
                                                value={field.value}
                                            >
                                                <SelectTrigger className="w-full">
                                                    <SelectValue placeholder="Seleciona a motivo dessa movimentação" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    <SelectGroup>
                                                        <SelectLabel>Motivo</SelectLabel>
                                                        <SelectItem value="Purchase">Compra</SelectItem>
                                                        <SelectItem value="Loss">Perda</SelectItem>
                                                        <SelectItem value="Adjustment">Ajuste</SelectItem>
                                                        <SelectItem value="Sale">Venda</SelectItem>
                                                        <SelectItem value="Reversal">Estorno</SelectItem>
                                                    </SelectGroup>
                                                </SelectContent>
                                            </Select>
                                            {errors.reason && <p className="text-red-500">{errors.reason.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            {/* <div className="flex items-center gap-3">
                                <Checkbox id="terms" />
                                <Label htmlFor="terms">Accept terms and conditions</Label>
                            </div> */}
                        </div>
                        <DialogFooter className="py-3">
                            <DialogClose asChild>
                                <Button variant="outline" className="cursor-pointer">Cancel</Button>
                            </DialogClose>
                            <Button type="submit" className="cursor-pointer">{stockMovement ? "Atualizar" : "Salvar"}</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>)
}