import { OrderSchema } from "@/schemas/OrderSchema";
import { apiServer } from "./apiServer";

export const addOrderServices = async(order: OrderSchema) =>{
    const api = await apiServer()
    const response = await api.post(`/api/order`, order);
    return response;
}
export const getOrderQuantity = async()=>{
    const api = await apiServer();
    const response = await api.get("/api/order/GetQuantity");
    return response;
}
export const getOrderQuantityByUserId = async(userId: number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/order/GetQuantityByUserId?userId=${userId}`);
    return response;
}
export const getOrdersByUserId = async(userId:number)=>{
    const api = await apiServer();
    const response = await api.get(`/api/order/GetByUserId?userId=${userId}`);
    return response;
}
export const getOrdersServices = async() =>{
    const api = await apiServer();
    const response = await api.get("");
    return response;
}
export const getOrdersAllByUserId = async(userId: number, page: number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/order?userId=${userId}&page=${page}`);
    return response;
}