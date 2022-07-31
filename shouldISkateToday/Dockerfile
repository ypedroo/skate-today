FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["shouldISkateToday.csproj", ""]
RUN dotnet restore "shouldISkateToday/shouldISkateToday.csproj"
COPY . .
WORKDIR "/src/shouldISkateToday"
RUN dotnet build "shouldISkateToday.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "shouldISkateToday.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "shouldISkateToday.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet shouldISkateToday.dll
