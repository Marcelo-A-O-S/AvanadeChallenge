import { useEffect, useState } from "react";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog";
import { Button } from "../ui/button";
import { Controller, useForm } from "react-hook-form";
import { InputGroup, InputGroupAddon, InputGroupInput, InputGroupText } from "../ui/input-group";
import { addCartSchema, AddCartSchema } from "@/schemas/ProductCatalogSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { CartProduct } from "@/types/cart-product";
import { updateSaleServices } from "@/services/client/saleService";
import { SaleSchema } from "@/schemas/SaleSchema";
import { toast } from "sonner";
type CartProductFormProps = {
    cartProduct : CartProduct | null,
    onSuccess: () => Promise<void>
}
export default function CartProductForm({cartProduct, onSuccess}: CartProductFormProps) {
    const [open, setOpen] = useState(false);
    const [cartProductData, setCartProductData ] = useState(cartProduct);
    const { control, watch, handleSubmit, setValue, formState: { errors } } = useForm<AddCartSchema>({
        resolver: zodResolver(addCartSchema),
        defaultValues: {
            quantity: 0,
            totalAmount: 0.0
        }
    });
    const quantity = watch("quantity");
    useEffect(() => {
        if(cartProductData !== null){
            onLoadingForm(cartProductData);
        }
    }, []);
    useEffect(() => {
        if (cartProduct?.price && quantity >= 0) {
            setValue("totalAmount", quantity * cartProduct.price);
        }
    }, [quantity, cartProduct, setValue]);
    const onLoadingForm = async(cartProduct: CartProduct) => {
        setValue("quantity", cartProduct.quantity);
    }
    const onSubmit = async(data: AddCartSchema) =>{
        if(!cartProductData)
            return 
        const sale : SaleSchema = {
            productId : cartProductData.productId,
            quantity : data.quantity,
            status : cartProductData.status,
            unitPrice: cartProductData.price,
            userId: cartProductData.userId,
        }
        const response = await updateSaleServices(cartProductData.id,sale);
        if(response.status !== 200 && response.status !== 201){
            console.log(response.data);
            return toast.error(response.data.message);
        }
        toast.success("Compra em andamento atualizada com sucesso!");
        await onSuccess();
    }

    return (
        <>
            <Dialog
                >
                <DialogTrigger asChild>
                    <Button className="cursor-pointer">Editar</Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <DialogHeader>
                            <DialogTitle>Editar Compra</DialogTitle>
                            <DialogDescription>
                                Descrição: {cartProduct?.description}
                            </DialogDescription>
                            <DialogDescription>
                                Preço: R$ {cartProduct?.price.toFixed(2)}
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
                                <Button variant="outline"  className="cursor-pointer">Cancel</Button>
                            </DialogClose>
                            <Button type="submit"  className="cursor-pointer">Salvar</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    )
}