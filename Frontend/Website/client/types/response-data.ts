type ResponseData<T>  = {
    status: number,
    data: T
}
type ReturnAcess = {
    message: string
}
export type ResponseAuth = ResponseData<ReturnAcess>;