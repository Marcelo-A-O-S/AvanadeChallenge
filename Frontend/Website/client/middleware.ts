import { NextRequest, NextResponse } from "next/server";
import { verifyToken } from "./lib/auth";
const pathPublics = [
  "/",
  "/auth/login",
  "/auth/register",
  "/api/auth/login",
  "/api/auth/register",
]
export async function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  const token = request.cookies.get("auth-token")?.value;
  if (!pathPublics.includes(pathname)) {
    if (token) {
      const user = await verifyToken(token);
      if (user) {
        return NextResponse.next();
      }
      request.cookies.delete("auth-token");
      return NextResponse.redirect(new URL("/auth/login", request.url));
    }
  }
  if (token) {
    const user = await verifyToken(token);
    if (user) {
      return NextResponse.redirect(new URL("/dashboard", request.url));
    }
    request.cookies.delete("auth-token");
  }
  return NextResponse.next();
}
export const config = {
  matcher: [
    "/dashboard/:path*",
    "/profile/:path*",
    "/auth/:path*",
    "/"
  ],
};