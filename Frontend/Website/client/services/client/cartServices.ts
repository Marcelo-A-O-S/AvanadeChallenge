import { apiClient } from "./apiClient"

export const getCartProductsServices = async(page: number, userId:number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/cart?userId=${userId}&page=${page}`);
    return response;
}