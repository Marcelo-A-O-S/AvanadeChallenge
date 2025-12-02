import { StockMovementSchema } from "@/schemas/StockMovementSchema";
import { apiServer } from "./apiServer";
export const getStockServices = async() => {
    const api = await apiServer();
    const response = await api.get("/api/stock");
    return response;
}
export const getStockPaginationServices = async(page:number) => {
    const api = await apiServer();
    const response = await api.get(`/api/stock?page=${page}`);
    return response;
}
export const getStockMovementByIdService = async(Id: number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/stock/${Id}`);
    return response;
}
export const addStockMovementService = async(stockMovement: StockMovementSchema) =>{
    const api = await apiServer();
    const response = await api.post(`/api/stock`,stockMovement);
    return response;
}
export const deleteStockMovementService = async(Id:number)=>{
    const api = await apiServer();
    const response = await api.delete(`/api/stock?Id=${Id}`);
    return response;
}
export const updateStockMovementService = async(Id:number, stockMovement: StockMovementSchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/stock/${Id}`, stockMovement);
    return response;
}
export const getStockMovementQuantity = async() =>{
    const api = await apiServer();
    const response = await api.get("/api/stock/GetQuantity");
    return response;
}