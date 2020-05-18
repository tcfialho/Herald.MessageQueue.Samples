#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY ["Accounts.Worker/Accounts.Worker.csproj", "Accounts.Worker/"]
COPY ["Accounts.Domain/Accounts.Domain.csproj", "Accounts.Domain/"]
RUN dotnet restore "Accounts.Worker/Accounts.Worker.csproj"
COPY . .
WORKDIR "/src/Accounts.Worker"
RUN dotnet build "Accounts.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Accounts.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Accounts.Worker.dll"]