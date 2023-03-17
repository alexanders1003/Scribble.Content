FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Scribble.Blogs.Web/Scribble.Blogs.Web.csproj", "Scribble.Blogs.Web/"]
COPY ["Scribble.Blogs.Contracts.Proto/Scribble.Blogs.Contracts.Proto.csproj", "Scribble.Blogs.Contracts.Proto/"]
COPY ["Scribble.Blogs.Infrastructure/Scribble.Blogs.Infrastructure.csproj", "Scribble.Blogs.Infrastructure/"]
COPY ["Scribble.Blogs.Models/Scribble.Blogs.Models.csproj", "Scribble.Blogs.Models/"]
COPY ["Scribble.Blogs.Contracts.Events/Scribble.Blogs.Contracts.Events.csproj", "Scribble.Blogs.Contracts.Events/"]
RUN dotnet restore "Scribble.Blogs.Web/Scribble.Blogs.Web.csproj"
COPY . .
WORKDIR "/src/Scribble.Blogs.Web"
RUN dotnet build "Scribble.Blogs.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Scribble.Blogs.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scribble.Blogs.Web.dll"]
