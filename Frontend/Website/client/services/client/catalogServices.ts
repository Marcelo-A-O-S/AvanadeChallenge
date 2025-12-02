import { apiClient } from "./apiClient"

export const getCatalogPaginationServices = async(page:number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/catalog?page=${page}`);
    return response;
}
export const getCatalogQuantity = async()=>{
    const api = await apiClient();
    const response = await api.get("/api/catalog/quantity");
    return response;
}