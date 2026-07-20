# ===============================================
# ETAPA 1: Compilăm Frontend-ul (ReactChat)
# ===============================================
FROM node:20-alpine AS frontend-build
WORKDIR /app/frontend

# Copiem package.json și instalăm dependențele
COPY ReactChat/package*.json ./
RUN npm install

# Copiem codul din ReactChat și generăm folderul "dist"
COPY ReactChat/ ./
RUN npm run build

# ===============================================
# ETAPA 2: Compilăm Backend-ul (C# Web API + proiectele conexe)
# ===============================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build
WORKDIR /src

# Copiem fișierele .csproj pentru a face "restore" eficient
COPY ["ChatServerWebApi/ChatServerWebApi.csproj", "ChatServerWebApi/"]
COPY ["ChatApp.Application/ChatApp.Application.csproj", "ChatApp.Application/"]
COPY ["ChatApp.Domain/ChatApp.Domain.csproj", "ChatApp.Domain/"]
COPY ["ChatApp.Infrastructure/ChatApp.Infrastructure.csproj", "ChatApp.Infrastructure/"]
COPY ["ChatApp.Shared/ChatApp.Shared.csproj", "ChatApp.Shared/"]

RUN dotnet restore "ChatServerWebApi/ChatServerWebApi.csproj"

# Copiem tot codul sursă și publicăm backend-ul
COPY . .
WORKDIR "/src/ChatServerWebApi"
RUN dotnet publish "ChatServerWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===============================================
# ETAPA 3: Imaginea finală de producție
# ===============================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiem executabilul C# compilat
COPY --from=backend-build /app/publish .

# Copiem fișierele statice din React în folderul wwwroot din C#
COPY --from=frontend-build /app/frontend/dist ./wwwroot

# Setăm portul pe care va asculta containerul
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ChatServerWebApi.dll"]