docker-compose -f .docker/docker-compose.yml down
xdg-open http://localhost:5000/swagger
open http://localhost:5000/swagger
start http://localhost:5000/swagger
xdg-open http://localhost:5002/hc
open http://localhost:5002/hc
start http://localhost:5002/hc
docker-compose -f .docker/docker-compose.yml up --force-recreate --no-deps --build