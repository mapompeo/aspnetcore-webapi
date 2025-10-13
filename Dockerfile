# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj e restaurar dependências
COPY ["APICatalogo.csproj", "./"]
RUN dotnet restore "APICatalogo.csproj"

# Copiar todo o código fonte
COPY . .

# Build do projeto
RUN dotnet build "APICatalogo.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "APICatalogo.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expor portas
EXPOSE 8080
EXPOSE 8081

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Criar diretório de logs
RUN mkdir -p /app/log

# Entrypoint
ENTRYPOINT ["dotnet", "ASPNETCORE-WEBAPI.dll"]