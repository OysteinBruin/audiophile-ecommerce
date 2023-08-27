import NextAuth, { NextAuthOptions } from "next-auth"
import IdentityServer4Provider from "next-auth/providers/identity-server4"
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6"

const handler = NextAuth({
    secret: "db3e1af5f2363bde9672de77d6994cff",
    providers: [
        DuendeIDS6Provider({
            clientId: "nextjs_web_app",
            clientSecret: "secret",
            issuer: "http://localhost:5105",
        }),
        IdentityServer4Provider({
            issuer: "http://localhost:5105",
            clientId: "nextjs_web_app",
            clientSecret: "secret",
        }),
    ],
});

export { handler as GET, handler as POST };
