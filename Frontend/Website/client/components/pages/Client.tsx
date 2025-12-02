import { useRouter, useSearchParams } from "next/navigation";
import { Pagination, PaginationContent, PaginationEllipsis, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from "../ui/pagination";
import { Separator } from "../ui/separator";

export default function Client() {
    const router = useRouter();
    const searchParams = useSearchParams();
    const pageValue = searchParams.get('page');
    const page = pageValue ? parseInt(pageValue) : 1;
    const goToPage = (page: number) => {
        const params = new URLSearchParams(searchParams.toString());
        params.set("page", page.toString());
        router.push(`?${params.toString()}`);
    };
    return (
        <>
            <main className="flex min-h-screen w-full flex-col items-center justify-between p-8 bg-white dark:bg-black sm:items-start">
                <section className="w-full flex flex-col gap-4 ">
                    <h1 className="text-2xl font-bold">Gerenciamento de Clientes</h1>
                    <div className="justify-end">

                    </div>
                    <Separator />
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-5">
                    </div>
                    <div>
                        <Pagination>
                            <PaginationContent>
                                <PaginationItem>
                                    <PaginationPrevious href="#"
                                        onClick={(e) => {
                                            e.preventDefault();
                                            goToPage((page || 2) - 1);
                                        }} />
                                </PaginationItem>
                                <PaginationItem>
                                    <PaginationLink href="#"
                                        onClick={(e) => {
                                            e.preventDefault();
                                            goToPage(1);
                                        }}>1</PaginationLink>
                                </PaginationItem>
                                <PaginationItem>
                                    <PaginationEllipsis />
                                </PaginationItem>
                                <PaginationItem>
                                    <PaginationNext
                                        href="#"
                                        onClick={(e) => {
                                            e.preventDefault();
                                            goToPage((page || 1) + 1);
                                        }}
                                    />
                                </PaginationItem>
                            </PaginationContent>
                        </Pagination>
                    </div>
                </section>
            </main>
        </>
    )
}