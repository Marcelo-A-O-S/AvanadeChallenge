import { apiServer } from "./apiServer";

export const getCatalogPaginationServices = async(page:number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/catalog?page=${page}`);
    return response;
}
export const getCatalogQuantity = async()=>{
    const api = await apiServer();
    const response = await api.get("/api/catalog/GetQuantity");
    return response;
}