FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 81
EXPOSE 444

ENV ASPNETCORE_URLS=http://+:81

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PaymentService/PaymentService.csproj", "PaymentService/"]
COPY ["CommonModels/CommonModels.csproj", "CommonModels/"]
RUN dotnet restore "PaymentService/PaymentService.csproj"
COPY . .
WORKDIR "/src/PaymentService"
RUN dotnet build "PaymentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.dll"]