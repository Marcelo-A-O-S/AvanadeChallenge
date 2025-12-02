import { apiServer } from "./apiServer";

export const getUserByEmail = async(email:string) =>{
    const api = await apiServer();
    const response = await api.get(`/api/Users/GetUserByEmail?email=${email}`);
    return response;
}
export const getUsersQuantity = async() =>{
    const api = await apiServer();
    const response = await api.get("/api/Users/GetQuantity");
    return response;
}