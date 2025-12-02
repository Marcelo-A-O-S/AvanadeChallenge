import { Controller, useForm } from "react-hook-form";
import { Button } from "../ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog";
import { ProductCatalog } from "@/types/product-catalog";
import { InputGroup, InputGroupAddon, InputGroupInput, InputGroupText } from "../ui/input-group";
import { ShoppingCart } from 'lucide-react';
import { addCartSchema, AddCartSchema } from "@/schemas/ProductCatalogSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { Sale } from "@/types/sale";
import { getUserByEmail } from "@/services/client/userServices";
import { useAuth } from "@/context/auth-context";
import { toast } from "sonner";
import { addSaleServices } from "@/services/client/saleService";
import { saleSchema } from "@/schemas/SaleSchema";
type ProductCatalogFormProps = {
    productCatalog: ProductCatalog | null,
    //onSuccess: () => Promise<void>;
};
export default function ProductCatalogForm({ productCatalog }: ProductCatalogFormProps) {
    const { user } = useAuth();
    const [open, setOpen] = useState(false);
    const { control, watch, handleSubmit, setValue, formState: { errors } } = useForm<AddCartSchema>({
        resolver: zodResolver(addCartSchema),
        defaultValues: {
            quantity: 0,
            totalAmount: 0.0
        }
    })
    const quantity = watch("quantity");
    useEffect(() => {
        if (productCatalog?.price && quantity >= 0) {
            setValue("totalAmount", quantity * productCatalog.price);
        }
    }, [quantity, productCatalog, setValue]);
    const onSubmit = async (data: AddCartSchema) => {

        if (productCatalog && user) {
            const responseUser = await getUserByEmail(user.email);
            if (responseUser.status !== 200 && responseUser.status !== 201) {
                console.log(responseUser.data);
                return toast.error("Erro ao adicionar ao carrinho!");
            }
            console.log(responseUser);
            const sale: Sale = {
                productId: productCatalog.id,
                quantity: data.quantity,
                status: "PROCESSING",
                unitPrice: productCatalog.price,
                userId: responseUser.data.id
            };
            const result = await saleSchema.safeParseAsync(sale);
            if(result.error){
                console.log("Error: ",result.error);
                return toast.error("Erro ao adicionar ao carrinho!");
            }
            const response = await addSaleServices(result.data);
            if (response.status !== 200 && response.status !== 201) {
                console.log(response.data);
                return toast.error("Erro ao adicionar ao carrinho!");
            }
            return toast.success("Adicionado ao carrinho com sucesso!");
        }
    }
    const onClear = async () => {
        setValue("quantity", 0);
        setValue("totalAmount", 0);
    }
    return (
        <>
            <Dialog
                open={open}
                onOpenChange={(isOpen) => {
                    setOpen(isOpen);
                    if (!isOpen) {
                        onClear();
                    }
                }}>
                <DialogTrigger asChild>
                    <Button className="cursor-pointer"><ShoppingCart /></Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader>
                            <DialogTitle>Adicionar ao Carrinho</DialogTitle>
                            <DialogDescription>
                                Descrição: {productCatalog?.description}
                            </DialogDescription>
                            <DialogDescription>
                                Preço: R$ {productCatalog?.price.toFixed(2)}
                            </DialogDescription>
                        </DialogHeader>
                        <div className="grid gap-4 py-1">
                            <Controller
                                name="quantity"
                                control={control}
                                render={({ field }) => (
                                    <div className="flex flex-col gap-2">
                                        <DialogDescription>
                                            Quantidade de items:
                                        </DialogDescription>
                                        <InputGroup>
                                            <InputGroupInput  {...field} 
                                            type="number" 
                                            onChange={e => field.onChange(parseInt(e.target.value))}
                                            placeholder="Informe a quantidade" />
                                            <InputGroupAddon>
                                            </InputGroupAddon>
                                        </InputGroup>
                                        {errors.quantity && <p className="text-red-500">{errors.quantity.message}</p>}
                                    </div>
                                )}
                            />
                            <Controller
                                name="totalAmount"
                                control={control}
                                render={({ field }) => (
                                    <div className="flex flex-col gap-2">
                                        <DialogDescription>
                                            Valor total:
                                        </DialogDescription>
                                        <InputGroup
                                            className="cursor-not-allowed">
                                            <InputGroupInput
                                                className="cursor-not-allowed"
                                                type="number"
                                                step="0.01"
                                                inputMode="decimal"
                                                disabled
                                                {...field} />
                                            <InputGroupAddon>
                                                <InputGroupText>$</InputGroupText>
                                            </InputGroupAddon>
                                        </InputGroup>
                                        {errors.totalAmount && <p className="text-red-500">{errors.totalAmount.message}</p>}
                                    </div>
                                )}
                            />
                        </div>
                        <DialogFooter className="py-3">
                            <DialogClose asChild>
                                <Button variant="outline" onClick={onClear} className="cursor-pointer">Cancel</Button>
                            </DialogClose>
                            <Button type="submit" className="cursor-pointer">Adicionar</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    )
}