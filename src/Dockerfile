FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5004
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Scribble.Content.Web/Scribble.Content.Web.csproj", "Scribble.Content.Web/"]
COPY ["Scribble.Content.Contracts.Proto/Scribble.Content.Contracts.Proto.csproj", "Scribble.Content.Contracts.Proto/"]
COPY ["Scribble.Content.Infrastructure/Scribble.Content.Infrastructure.csproj", "Scribble.Content.Infrastructure/"]
COPY ["Scribble.Content.Contracts.Events/Scribble.Content.Contracts.Events.csproj", "Scribble.Content.Contracts.Events/"]
COPY ["Scribble.Content.Models/Scribble.Content.Models.csproj", "Scribble.Content.Models/"]
RUN dotnet restore "Scribble.Content.Web/Scribble.Content.Web.csproj"
COPY . .
WORKDIR "/src/Scribble.Content.Web"
RUN dotnet build "Scribble.Content.Web.csproj" -c Release -o /app/build

RUN sleep 30

FROM build AS publish
RUN dotnet publish "Scribble.Content.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scribble.Content.Web.dll"]
