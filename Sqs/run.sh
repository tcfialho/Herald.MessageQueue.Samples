docker-compose -f .docker/docker-compose.yml down
docker-compose -f .docker/docker-compose.yml up -d
sleep 10
dotnet run --project Accounts.Api/Accounts.Api/Accounts.Api.csproj & dotnet run --project Accounts.Worker/Accounts.Worker/Accounts.Worker.csproj
docker-compose -f .docker/docker-compose.yml down