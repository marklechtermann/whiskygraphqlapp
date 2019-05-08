# You need Docker >=19.03.0 Pre-Release to support --memory on LCOW!

FROM mcr.microsoft.com/dotnet/core/sdk:2.1.603-alpine3.9 AS build

COPY /src /app

WORKDIR /app

RUN apk update && \
    apk add nodejs npm && \
    dotnet publish -c Release


FROM mcr.microsoft.com/dotnet/core/aspnet:2.1.10-alpine3.9

COPY --from=build /app/bin/Release/netcoreapp2.1/publish/ /app

WORKDIR /app

EXPOSE 5000

CMD ["dotnet", "whisky.dll"]