"use server";

import { cookies } from "next/headers";

export async function removeAuthToken() {
    const cookieStorie = await cookies();
    await cookieStorie.delete("auth-token");
}
