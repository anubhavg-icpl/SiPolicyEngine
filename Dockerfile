FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -r linux-x64 -o /app

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-preview
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["/app/SiPolicyEngine"]
