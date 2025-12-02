import toast from "react-hot-toast";
type ToastProps = {
    id:string,
    message:string,
    status: number
};
export function ToastCustom({id,message,status  }: ToastProps) {

    return (
        <div
            className={`bg-white dark:bg-black w-full h-full`}
        >
            <p className="text-primary">{message}</p>
        </div>
    )
}