FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .


RUN dotnet restore "WebApi/WebApi.csproj"
WORKDIR "/src/."
COPY . .
RUN dotnet build "WebApi/WebApi.csproj" -c Release -o /app/build


FROM build as publish
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebApi.dll"]