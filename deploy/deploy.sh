echo "Deploy started.."
mkdir -p ./data
chmod 755 ./data
docker-compose pull
docker-compose -p distvisor up --force-recreate --build -d 
docker image prune -f