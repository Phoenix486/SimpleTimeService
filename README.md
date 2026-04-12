# SimpleTimeService

A lightweight, containerized microservice that returns the current timestamp and visitor IP address as JSON. This is a minimal .NET Core web API designed to demonstrate containerization best practices and cloud-native deployment patterns.

## 📋 Overview

**SimpleTimeService** is a production-ready ASP.NET Core microservice built to showcase DevOps best practices. When you access the `/` endpoint, it returns a JSON response with:
- `timestamp`: Current date and time (ISO 8601 format)
- `ip`: The IP address of the HTTP request visitor

### Example Response
```json
{
  "timestamp": "2026-04-12T09:22:20.2683370+05:30",
  "ip": "127.0.0.1"
}
```

## 🎯 Purpose

This service is designed to:
- Demonstrate a cloud-native, containerized microservice architecture
- Showcase security best practices (non-root user execution, minimal image footprint)
- Serve as a deployable service for the accompanying Terraform/OpenTofu infrastructure
- Provide a testable endpoint for CI/CD pipeline validation

## 📦 Prerequisites

### Required Tools
- **.NET 9.0 SDK** - [Install](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker** - [Install](https://www.docker.com/products/docker-desktop)
- **Docker Hub Account** - [Create](https://hub.docker.com/) (for publishing images)
- **Azure CLI** *(optional, for ACR)* - [Install](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)

### System Requirements
- 2GB+ RAM available
- ~500MB disk space for build artifacts
- Internet connection for downloading dependencies

## 🚀 Quick Start - Local Development

### 1. Clone and Navigate
```bash
git clone https://github.com/YOUR-ORG/SimpleTimeService.git
cd SimpleTimeService
```

### 2. Build the Application
```bash
dotnet build
```

### 3. Run Locally
```bash
dotnet run
```

The application will start on `http://localhost:5207`

### 4. Test the Endpoint
```bash
curl http://localhost:5207/

# Expected output:
# {
#   "timestamp": "2026-04-12T09:22:20.2683370+05:30",
#   "ip": "127.0.0.1"
# }
```

## 🐳 Docker - Build, Run, and Publish

### Build the Docker Image

```bash
docker build -t simpletimeservice:latest .
```

**Image Size**: Optimized to < 100MB using Alpine Linux and multi-stage builds

### Run the Container Locally

```bash
docker run -p 8080:80 simpletimeservice:latest
```

Test the running container:
```bash
curl http://localhost:8080/
```

### Verify Non-Root User Execution

```bash
docker run --rm simpletimeservice:latest whoami
# Output should be: appuser (not root)
```

### Push to Docker Hub

```bash
# Tag the image with your Docker Hub username
docker tag simpletimeservice:latest YOUR-USERNAME/simpletimeservice:latest

# Login to Docker Hub
docker login

# Push to Docker Hub
docker push YOUR-USERNAME/simpletimeservice:latest
```

### Push to Azure Container Registry (ACR)

#### 1. Create an ACR instance (if not already created)

```bash
az acr create --resource-group <resource-group-name> \
  --name <acr-name> \
  --sku Basic
```

#### 2. Login to ACR

```bash
az acr login --name <acr-name>
```

#### 3. Tag and push the image

```bash
# Tag the image
docker tag simpletimeservice:latest <acr-name>.azurecr.io/simpletimeservice:latest

# Push to ACR
docker push <acr-name>.azurecr.io/simpletimeservice:latest
```

## 🔒 Security Features

- ✅ **Non-root User**: Container runs as `appuser` (UID 1001, GID 1001)
- ✅ **Minimal Image Size**: Alpine-based .NET runtime (~100MB)
- ✅ **Multi-stage Build**: Build dependencies excluded from final image
- ✅ **No Hardcoded Secrets**: All configuration via environment variables
- ✅ **Read-only Filesystem** *(recommended for K8s)*: Can be enforced at runtime
- ✅ **Resource Limits**: Easily configurable when deploying (see IaC-SimpleTimeService)

## 🔄 CI/CD Pipeline

This repository includes automated workflows:

- **Trivy Security Scan**: Scans container images for vulnerabilities on every PR
- **ACR Push and Helm Update**: Automatically builds and pushes images to ACR on merge to main
- Requires federated credentials configured with Azure (see [IaC-SimpleTimeService](../IaC-SimpleTimeService) for deployment details)

### GitHub Actions Workflows

Located in `.github/workflows/`:
- `ACR push and Helm Update.yml` - Builds image and pushes to ACR on main branch merge
- `trivy scan.yml` - Security scanning for vulnerabilities

## 📊 Application Architecture

```
Browser Request
    ↓
ASP.NET Core Kestrel Server (Port 80 inside container)
    ↓
Root "/" Endpoint Handler
    ↓
Captures: Timestamp + Visitor IP
    ↓
Returns JSON Response
```

## 🧪 Testing

### Local Testing
```bash
# Build and run
dotnet build
dotnet run

# Test endpoint
curl -X GET http://localhost:5207/
```

### Docker Testing
```bash
# Build image
docker build -t simpletimeservice:latest .

# Run container
docker run -p 8080:80 simpletimeservice:latest

# Test endpoint
curl -X GET http://localhost:8080/
```

### Container Security Verification
```bash
# Ensure running as non-root
docker run -rm ampletimeservice:latest id
# Output should show uid=1001

# Check image size
docker image ls simpletimeservice:latest
```

## 📝 Project Structure

```
SimpleTimeService/
├── Program.cs                      # ASP.NET Core application entry point
├── SimpleTimeService.csproj        # .NET project file
├── Dockerfile                      # Multi-stage Docker build
├── .dockerignore                   # Docker build exclusions
├── helm-chart/                     # Kubernetes Helm deployment manifests
│   ├── Chart.yaml
│   ├── values.yaml
│   └── templates/
│       ├── deployment.yaml
│       ├── service.yaml
│       ├── ingress.yaml
│       └── ...
├── appsettings.json               # Application configuration
├── appsettings.Development.json   # Development-specific settings
├── Properties/
│   └── launchSettings.json        # Launch profiles
└── README.md                      # This file
```

## 🌐 Deployment

### Kubernetes Deployment

This service is designed to be deployed to Kubernetes (EKS, GKE, AKS, etc.). See the accompanying [IaC-SimpleTimeService](../IaC-SimpleTimeService) repository for complete infrastructure-as-code deployment instructions.

### Direct Container Deployment

For cloud services like Azure Container Apps or AWS Fargate:
1. Push image to your chosen registry
2. Configure the service to use the image URI
3. Expose on port 80
4. Run as UID 1001 (appuser)

## 🔐 Authentication & Credentials

**No credentials are stored in this repository.** All authentication is handled through:
- GitHub Actions secrets (for CI/CD)
- Azure Managed Identities (for K8s deployment)
- Container registry credentials (managed by Azure/Docker)

Refer to the [IaC-SimpleTimeService](../IaC-SimpleTimeService) README for setting up federated credentials and OIDC authentication.

## 📚 Environment Variables

The application currently does not require environment variables. However, when deployed to Kubernetes, consider configuring:

```yaml
ASPNETCORE_ENVIRONMENT: Production
ASPNETCORE_URLS: http://+:80
```

These are set in the Dockerfile and Kubernetes manifests respectively.

## ❌ Troubleshooting

### Port Already in Use
```bash
# Error: Address already in use
# Solution: Use a different port
docker run -p 8081:80 simpletimeservice:latest
```

### Docker Build Fails
```bash
# Ensure Docker daemon is running
docker ps

# Check available disk space
df -h

# Clean up Docker artifacts
docker system prune -a
```

### Cannot Connect to Local Service
```bash
# Verify container is running
docker ps

# Check logs
docker logs <container-id>

# Test locally
curl -v http://localhost:8080/
```

### ACR Login Issues
```bash
# Ensure you have az CLI installed and authenticated
az login

# Create ACR if needed
az acr create --resource-group mygroup --name myacr --sku Basic
```

## 🧹 Cleanup & Cost Management

⚠️ **Important**: After testing, clean up to avoid unnecessary charges.

### Local Cleanup
```bash
# Remove local image
docker image rm simpletimeservice:latest

# Remove dangling images
docker image prune -a
```

### ACR Cleanup
```bash
# Delete repository from ACR
az acr repository delete --name <acr-name> --repository simpletimeservice

# Or delete entire ACR
az acr delete --resource-group <resource-group> --name <acr-name>
```

### Full Infrastructure Cleanup
For cleaning up Kubernetes or cloud infrastructure, see the [IaC-SimpleTimeService](../IaC-SimpleTimeService) README section on infrastructure destruction.

## 📖 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [.NET in Containers](https://learn.microsoft.com/en-us/dotnet/core/docker/introduction)
- [Kubernetes Best Practices](https://kubernetes.io/docs/concepts/configuration/overview/)

## 📄 License

This project is provided as-is for educational and evaluation purposes.

## 👥 Support

For issues or questions, please refer to the repository's issue tracker or contact the Particle41 DevOps team.