# Stage 1: Build Frontend
FROM node:20-alpine AS frontend-build
WORKDIR /app/frontend
COPY Frontend/package*.json ./
RUN npm install
COPY Frontend/ ./
RUN npm run build -- --configuration production

# Stage 2: Build Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app/backend
COPY Backend/*.csproj ./
RUN dotnet restore
COPY Backend/ ./
RUN dotnet publish -c Release -o out

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /app/backend/out .
COPY --from=frontend-build /app/frontend/dist/frontend/browser ./wwwroot

# Expose port (Railway uses PORT env var)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 80

ENTRYPOINT ["dotnet", "Backend.dll"]
