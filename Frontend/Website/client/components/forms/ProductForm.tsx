"use client"
import { productSchema, ProductSchema } from "@/schemas/ProductSchema";
import { Button } from "../ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog";
import { zodResolver } from "@hookform/resolvers/zod"
import { Controller, useForm } from "react-hook-form"
import { InputGroup, InputGroupAddon, InputGroupInput, InputGroupTextarea, InputGroupText } from "../ui/input-group";
import { Package, FilePenLine, FilePen, Boxes } from "lucide-react";
import { Label } from "../ui/label";
import { addProductService, getProductByIdService, updateProductService } from "@/services/client/productServices";
import { toast } from "sonner";
import { useEffect, useState } from "react";
import { Product } from "@/types/product";
type ProductFormProps = {
    product: Product | null,
    onSuccess:()=> Promise<void>;
};
export default function ProductForm({ product, onSuccess }: ProductFormProps) {
    const [loading, setLoading] = useState(false);
    const [productData, setProductData] = useState(product);
    const { control, handleSubmit, formState: { errors }, setValue } = useForm<ProductSchema>({
        resolver: zodResolver(productSchema),
        defaultValues: {
            name: '',
            description: '',
            price: 0.0,
            quantity: 0,
            minimunStock: 0
        }
    });
    useEffect(() => {
        if (productData !== null) {
            onLoadingForm(productData)
        }
    }, [])
    const onLoadingForm = async (product: Product) => {
        try {
            setLoading(true);
            setValue("name", product.name);
            setValue("description", product.description);
            setValue("price", product.price);
            setValue("quantity", product.quantity);
            setValue("minimunStock", product.minimunStock);
        } finally {
            setLoading(false);
        }
    }
    const onSubmit = async (data: ProductSchema) => {
        let response = null;
        if (productData) {
            console.log(`Atualizando o produto com o Id: ${productData.id}`);
            response = await updateProductService(productData.id, data);
            if (response.status !== 200 && response.status !== 201) {
                console.log(response.data);
                return toast.error("Erro ao salvar produto:", response.data.message);
            }
            await onSuccess();
            console.log(response.data);
            return toast.success("Produto a atualizado com sucesso!");
        } else {
            response = await addProductService(data);
            if (response.status !== 200 && response.status !== 201) {
                console.log(response.data);
                return toast.error("Erro ao salvar produto:", response.data.message);
            }
            await onSuccess();
            return toast.success("Produto salvo com sucesso!");
        }
    }
    return (
        <>
            <Dialog>
                <DialogTrigger asChild>
                    <Button className="cursor-pointer">{product ? "Editar " : "Criar produto"}</Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader>
                            <DialogTitle>{product ? "Editar Produto" : "Criar Produto"}</DialogTitle>
                            <DialogDescription>
                                Informe os dados abaixo:
                            </DialogDescription>
                        </DialogHeader>
                        <div className="grid gap-4">
                            <div className="grid gap-3">
                                <Controller
                                    name="name"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Produto:
                                            </DialogDescription>
                                            <InputGroup>
                                                <InputGroupInput {...field} placeholder="Informe o nome do produto" />
                                                <InputGroupAddon>
                                                    <FilePen />
                                                </InputGroupAddon>
                                            </InputGroup>
                                            {errors.name && <p className="text-red-500">{errors.name.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div className="grid gap-3">
                                <Controller
                                    name="description"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Descrição:
                                            </DialogDescription>
                                            <InputGroup>
                                                <InputGroupTextarea  {...field} placeholder="Informe uma descrição do produto" />
                                                <InputGroupAddon>
                                                    <FilePenLine />
                                                </InputGroupAddon>
                                            </InputGroup>
                                            {errors.description && <p className="text-red-500">{errors.description.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div className="grid gap-3">
                                <Controller
                                    name="price"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Preço:
                                            </DialogDescription>
                                            <InputGroup>
                                                <InputGroupInput
                                                    {...field}
                                                    type="number"
                                                    step="0.01"
                                                    inputMode="decimal"
                                                    onChange={e => field.onChange(parseFloat(e.target.value))}
                                                    placeholder="Informe um preço para o produto" />
                                                <InputGroupAddon>
                                                    <InputGroupText>$</InputGroupText>
                                                </InputGroupAddon>
                                            </InputGroup>
                                            {errors.price && <p className="text-red-500">{errors.price.message}</p>}
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
                                    name="minimunStock"
                                    control={control}
                                    render={({ field }) => (
                                        <div className="flex flex-col gap-2">
                                            <DialogDescription>
                                                Quantidade minima:
                                            </DialogDescription>
                                            <InputGroup>
                                                <InputGroupInput
                                                    {...field}
                                                    type="number"
                                                    onChange={e => field.onChange(parseInt(e.target.value))}
                                                    placeholder="Informe uma quantidade minima para o produto" />
                                                <InputGroupAddon>
                                                    <Boxes />
                                                </InputGroupAddon>
                                            </InputGroup>
                                            {errors.minimunStock && <p className="text-red-500">{errors.minimunStock.message}</p>}
                                        </div>
                                    )}
                                />
                            </div>
                        </div>
                        <DialogFooter className="py-3">
                            <DialogClose asChild>
                                <Button variant="outline" className="cursor-pointer">Cancel</Button>
                            </DialogClose>
                            <Button type="submit" className="cursor-pointer">{product ? "Atualizar" : "Salvar"}</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>)
} 