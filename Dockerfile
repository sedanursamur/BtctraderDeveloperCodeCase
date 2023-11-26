#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecurringBitcoinPurchaseInstructions.csproj", "."]
RUN dotnet restore "./RecurringBitcoinPurchaseInstructions.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "RecurringBitcoinPurchaseInstructions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecurringBitcoinPurchaseInstructions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecurringBitcoinPurchaseInstructions.dll"]