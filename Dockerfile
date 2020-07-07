FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY . .
WORKDIR /src/Hktn.Api
RUN dotnet publish -c Release -o ../../out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

RUN apt-get update &&\
    apt-get install -y libc++1

WORKDIR /app

COPY --from=build /out .

ENTRYPOINT ["dotnet", "Hktn.Api.dll"]
