FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY src/FootballSubscriber.Api/FootballSubscriber.Api.csproj src/FootballSubscriber.Api/
COPY src/FootballSubscriber.Core/FootballSubscriber.Core.csproj src/FootballSubscriber.Core/
COPY src/FootballSubscriber.Infrastructure/FootballSubscriber.Infrastructure.csproj src/FootballSubscriber.Infrastructure/
COPY test/FootballSubscriber.Test/FootballSubscriber.Test.csproj test/FootballSubscriber.Test/
COPY FootballSubscriber.sln .

RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as run

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "FootballSubscriber.Api.dll"]

