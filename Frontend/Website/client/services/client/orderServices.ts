import { OrderSchema } from "@/schemas/OrderSchema"
import { apiClient } from "./apiClient"

export const addOrderServices = async(order: OrderSchema) =>{
    const api = await apiClient()
    const response = await api.post(`/api/order`, order);
    return response;
}
export const getOrderQuantity = async()=>{
    const api = await apiClient();
    const response = await api.get("/api/order/quantity");
    return response;
}
export const getOrderQuantityByUserId = async(userId: number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/order/quantity?userId=${userId}`);
    return response;
}
export const getOrdersByUserId = async(userId:number)=>{
    const api = await apiClient();
    const response = await api.get(`/api/order?userId=${userId}`);
    return response;
}
export const getOrdersServices = async() =>{
    const api = await apiClient();
    const response = await api.get("");
    return response;
}
export const getOrdersAllByUserId = async(userId: number, page: number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/order?userId=${userId}&page=${page}`);
    return response;
}