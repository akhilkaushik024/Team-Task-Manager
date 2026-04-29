# Stage 1: Build Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app
COPY Backend/Backend.csproj Backend/
RUN dotnet restore Backend/Backend.csproj
COPY Backend/ Backend/
RUN dotnet publish Backend/Backend.csproj -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /app/out .
# Copy pre-built frontend files directly from the repository context
COPY Frontend/dist/frontend/browser ./wwwroot

# Expose port (Railway provides PORT env var)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Backend.dll"]
