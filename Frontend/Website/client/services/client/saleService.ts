import { Sale } from "@/types/sale";
import { apiClient } from "./apiClient"
import { SaleSchema } from "@/schemas/SaleSchema";

export const getSalesService = async() =>{
    const api = await apiClient();
    const response = await api.get("/api/sales");
    return response;
}
export const getSalesPaginationService = async(page: number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/sales?page=${page}`);
    return response;
}
export const getSaleByIdService = async(id:number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/sales/${id}`);
    return response;
}
export const addSaleServices = async(sale: SaleSchema)=>{
    const api = await apiClient();
    const response = await api.post("/api/sales",sale);
    return response;
}
export const updateSaleServices = async(id:number, sale: SaleSchema) =>{
    const api = await apiClient();
    const response = await api.put(`/api/sales/${id}`, sale);
    return response;
}
export const deleteSaleServices = async(id:number) =>{
    const api = await apiClient();
    const response = await api.delete(`/api/sales?Id=${id}`);
    return response;
}
export const getSalesQuantity = async() =>{
    const api = await apiClient();
    const response = await api.get("/api/sales/quantity");
    return response;
}
export const getSalesQuantityByUserId = async(userId: number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/sales/quantity?userId=${userId}`);
    return response;
}