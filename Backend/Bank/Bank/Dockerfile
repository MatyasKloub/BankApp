#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bank/Bank.csproj", "Bank/"]
RUN dotnet restore "Bank/Bank.csproj"
COPY . .
WORKDIR "/src/Bank"
RUN dotnet build "Bank.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Bank/email.txt /app/email.txt

COPY Bank/kurz.txt /app/kurz.txt

ENTRYPOINT ["dotnet", "Bank.dll"]