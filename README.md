# SimpleTimeService

A minimal .NET Core REST API that returns JSON with the current timestamp and visitor IP. Containerized and designed for cloud deployment.

## 🚀 Quick Start

### Local Development
```bash
# Prerequisites: .NET 9.0 SDK, Docker

# Build
dotnet build

# Run locally
dotnet run
# Access: http://localhost:5207/
```
```

### Test
```bash
curl http://localhost:5207/
```

### Docker
```bash
# Build
docker build -t simpletimeservice:latest .

# Run
docker run -p 8080:80 simpletimeservice:latest

# Test
curl http://localhost:8080/

# Verify non-root user
docker run --rm simpletimeservice:latest whoami
# Output: appuser
```

### Push to Azure Container Registry
```bash
az acr login --name <acr-name>
docker tag simpletimeservice:latest <acr-name>.azurecr.io/simpletimeservice:latest
docker push <acr-name>.azurecr.io/simpletimeservice:latest
```

## 🔒 Security

✅ Runs as non-root user (`appuser`, UID 1001)  
✅ Multi-stage build (~100MB image size)  
✅ No hardcoded secrets  

## 📝 Project Files

- **Program.cs** — ASP.NET Core application  
- **Dockerfile** — Multi-stage Docker build  
- **helm-chart/** — Kubernetes deployment manifests  

## 🌐 Deployment

See [IaC-SimpleTimeService](../IaC-SimpleTimeService) for Kubernetes (AKS) deployment instructions.

## 📖 Response Example

```json
{
  "timestamp": "2026-04-12T09:22:20.2683370+05:30",
  "ip": "127.0.0.1"
}
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