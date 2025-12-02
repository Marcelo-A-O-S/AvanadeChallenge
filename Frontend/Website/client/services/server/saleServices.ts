import { SaleSchema } from "@/schemas/SaleSchema";
import { apiServer } from "./apiServer"

export const getSalesService = async () =>{
    const api = await apiServer();
    const response = await api.get("/api/sale");
    return response;
}
export const getSalesPaginationService = async(page: number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/Sales?page=${page}`);
    return response;
}
export const getSaleByIdService = async(id:number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/Sales/${id}`);
    return response;
}
export const addSaleServices = async(sale: SaleSchema)=>{
    const api = await apiServer();
    const response = await api.post("/api/Sales",sale);
    return response;
}
export const updateSaleServices = async(id:number, sale: SaleSchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/Sales/${id}`, sale);
    return response;
}
export const deleteSaleServices = async(id:number) =>{
    const api = await apiServer();
    const response = await api.delete(`/api/sales?Id=${id}`);
    return response;
}
export const getSalesQuantity = async() =>{
    const api = await apiServer();
    const response = await api.get("/api/sales/GetQuantity");
    return response;
}
export const getSalesQuantityByUserId = async(userId: number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/sales/GetQuantityInProgressByUserId?userId=${userId}`);
    return response;
}