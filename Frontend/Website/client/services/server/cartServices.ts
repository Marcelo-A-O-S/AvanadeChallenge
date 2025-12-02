import { apiServer } from "./apiServer";

export const getCartProductsServices = async(page: number, userId:number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/cart?userId=${userId}&page=${page}`);
    return response;
}