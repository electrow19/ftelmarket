Pour lancer le projet il suffit de faire :
dans FTEL-MARKET-FRONT-main :
docker compose up -d (et pour stop docker compose down)
dans FTELMarket :
dotnet run --project=FTELMarket.csproj (normalement il y a un fichier DbInitializer.cs qui permet de faire la migration)
