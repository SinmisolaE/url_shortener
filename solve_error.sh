# Stop everything
docker compose down

# Start MySQL and Redis first
docker compose up -d urlshort.db urlshort.redis

# Wait for MySQL to be ready (check logs)
docker compose logs urlshort.db

# When you see "MySQL init process done. Ready for start up." wait 10 more seconds
sleep 30

# Then start your API
docker compose up -d urlshort.api

# Check API logs
docker compose logs urlshort.api