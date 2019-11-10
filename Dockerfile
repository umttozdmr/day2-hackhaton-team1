FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

COPY . .
WORKDIR /src/Hktn.Api
RUN dotnet publish -c Release -o ../../out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime

WORKDIR /app

COPY --from=build /out .

ENTRYPOINT ["dotnet", "Hktn.Api.dll"]
