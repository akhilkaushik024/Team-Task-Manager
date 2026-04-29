# Use SDK as base to build and run (slightly heavier but more reliable for paths)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything
COPY . .

# Build Backend
RUN dotnet publish Backend/Backend.csproj -c Release -o out

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
# Copy the pre-built frontend directly
COPY --from=build /app/Frontend/dist/frontend/browser ./wwwroot

# Expose port
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Backend.dll"]
