#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development
cd src
dotnet watch run
cd ..