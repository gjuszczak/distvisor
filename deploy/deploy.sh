#!/usr/bin/env bash
source .env
echo "Deploy started.."
mkdir -p ./data
chmod 755 ./data

if [ "$DB_UPDATE_STRATEGY" = "MigrateToLatest" ]; then
   cp ../$PREVIOUS_VERSION/data/distvisor.db ./data/distvisor.db
   echo "distvisor.db - MigrateToLatest - from ${PREVIOUS_VERSION}"
fi

docker-compose pull
docker-compose -p distvisor up --force-recreate --build -d 
docker image prune -f
