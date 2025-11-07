# ===== Build stage =====

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln ./

# Copy project file and restore first (cache-friendly)
COPY EFCoreExample.csproj ./
RUN dotnet restore

# Copy the rest and publish
COPY . .
RUN dotnet publish -c Release -o /publish /p:UseAppHost=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /src
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "EFCoreExample.dll"]