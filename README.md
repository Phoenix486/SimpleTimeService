# SimpleTimeService

A lightweight, containerized microservice that returns the current timestamp and visitor IP address as JSON.

## Overview

**SimpleTimeService** is a minimal web service built with ASP.NET Core. When you access the `/` endpoint, it returns a JSON response with:
- `timestamp`: Current date and time (ISO 8601 format)
- `ip`: The IP address of the HTTP request visitor

### Example Response
```json
{
  "timestamp": "2026-04-12T09:22:20.2683370+05:30",
  "ip": "127.0.0.1"
}
```

## Prerequisites

- **.NET 9.0 SDK** - [Install](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker** (for containerization) - [Install](https://www.docker.com/products/docker-desktop)

## Quick Start - Local Development

### Build the Application

```bash
dotnet build
```

### Run Locally

```bash
dotnet run
```

The application will start on `http://localhost:5207`

### Test the Endpoint

```bash
curl http://localhost:5207/
```

## Docker - Build and Run

### Build the Docker Image

```bash
docker build -t simpletimeservice:latest .
```

### Run the Container

```bash
docker run -p 5207:5207 simpletimeservice:latest
```

### Test the Container

```bash
curl http://localhost:5207/
```

### Push to Azure Container Registry (ACR)

Create an ACR instance (if not already created):

```bash
az acr create --resource-group <rg-name> --name <acr-name> --sku Basic
```

Login to ACR:

```bash
az acr login --name <acr-name>
```

Tag and push the image:

```bash
docker tag simpletimeservice:latest <acr-name>.azurecr.io/simpletimeservice:latest
docker push <acr-name>.azurecr.io/simpletimeservice:latest
```

## Security Features

- ✅ **Non-root User**: Container runs as `appuser` (UID 1001)
- ✅ **Minimal Image Size**: Alpine-based .NET runtime
- ✅ **Multi-stage Build**: Build dependencies excluded from final image
- ✅ **No Secrets in Code**: All configuration via environment variables or cloud provider authentication