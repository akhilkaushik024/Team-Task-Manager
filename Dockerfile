# Stage 1: Build Frontend
FROM node:20-slim AS frontend-build
WORKDIR /app/frontend
# Install pnpm for memory efficiency
RUN npm install -g pnpm
COPY Frontend/package*.json ./
RUN pnpm install --prod=false
COPY Frontend/ ./
# Increase memory limit for the Angular build itself
RUN node --max-old-space-size=1024 ./node_modules/@angular/cli/bin/ng build --configuration production

# Stage 2: Build Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app
COPY Backend/Backend.csproj Backend/
RUN dotnet restore Backend/Backend.csproj
COPY Backend/ Backend/
RUN dotnet publish Backend/Backend.csproj -c Release -o out

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /app/out .
COPY --from=frontend-build /app/frontend/dist/frontend/browser ./wwwroot

# Expose port (Railway provides PORT env var)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Backend.dll"]
