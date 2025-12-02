import { apiClient } from "./apiClient"

export const getUserByEmail = async(email:string) =>{
    const api = await apiClient();
    const response = await api.get(`/api/users?email=${email}`);
    return response;
}
export const getUsersQuantity = async() =>{
    const api = await apiClient();
    const response = await api.get("/api/users/quantity");
    return response;
}