# WhiskyApp with .NET Core, GraphQL and Angular

A small demo application for the **Dot NET Cologne 2019**  (https://dotnet-cologne.de/Vortraege.ashx#graphql)

## How to run this application with Docker

`docker run -p 5000:5000 marklechtermann/whiskygraphqlapp`

https://hub.docker.com/r/marklechtermann/whiskygraphqlapp

## How to run this application

You need .NET Core >= 2.1

`./run.sh`

and for the Frontend

`cd ClientApp; ng serve`

Open http://localhost:5000 in your browser.

## How to run publish the application

`dotnet publish ./src/whisky.csproj -c Release`

## Content

Backend:
* .NET Core 
* ASP.NET Core WebAPI
* GraphQL

Frontend:
* Angular
* Apollo GraphQL Client



