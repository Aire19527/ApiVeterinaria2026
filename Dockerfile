# Etapa 1: compila el proyecto. Esta imagen trae el SDK y no se publica.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# El contexto es la raíz del repo porque la API referencia Domain, Core, Entity y Common.
COPY . .
# Publica solo la API en Release. UseAppHost=false evita un ejecutable nativo que no hace falta.
RUN dotnet publish API.Veterinaria/API.Veterinaria.csproj -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: imagen final, más chica. Solo tiene el runtime y lo ya publicado.
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
# La API escucha HTTP en el 8080 dentro del contenedor. El + acepta llamadas de cualquier interfaz.
ENV ASPNETCORE_URLS=http://+:8080
# Documenta el puerto. No lo abre en el PC; eso lo hace el proxy del compose.
EXPOSE 8080
ENTRYPOINT ["dotnet", "API.Veterinaria.dll"]
