import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  images:{
    remotePatterns:[{
      hostname: "digitalassets.avanade.com",
      protocol: "https",
      pathname: "/api/public/content/**"
    }]
  }
};

export default nextConfig;
