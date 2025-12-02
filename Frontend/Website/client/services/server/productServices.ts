import { ProductSchema } from "@/schemas/ProductSchema";
import { apiServer } from "./apiServer";

export const getProductsPagination = async(page: number) => {
    const api = await apiServer();
    const response = await api.get(`/api/product?page=${page}`);
    return response;
}
export const getProducts = async() => {
    const api = await apiServer();
    const response = await api.get(`/api/product`);
    return response;
}
export const addProductService = async(product: ProductSchema) =>{
    const api = await apiServer();
    const response = await api.post("/api/product",product);
    return response;
}
export const deleteProductService = async(id:number) =>{
    const api = await apiServer();
    const response = await api.delete(`/api/product?Id=${id}`);
    return response;
}
export const getProductByIdService = async(id:number)=>{
    const api = await apiServer();
    const response = await api.get(`/api/product/${id}`);
    return response;
}
export const updateProductService = async(id:number,product:ProductSchema)=>{
    const api = await apiServer();
    const response = await api.put(`/api/product/${id}`,product);
    return response;
}
export const getProductQuantity = async()=>{
    const api = await apiServer();
    const response = await api.get("/api/product/GetQuantity");
    return response;
}