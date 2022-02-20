# Chess

## Setup

```bash
docker run --rm --name pg --net=host -e POSTGRES_PASSWORD=secret -d postgres:14

dotnet tool install --global dotnet-ef
dotnet ef database update
dotnet run
```
