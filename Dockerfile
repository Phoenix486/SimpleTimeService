FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /App

# Create a non-root user
RUN addgroup -g 1001 appgroup && \
    adduser -D -u 1001 -G appgroup appuser

COPY --from=build --chown=appuser:appgroup /App/out .

# Configure app to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

USER appuser

EXPOSE 8080
ENTRYPOINT ["dotnet", "SimpleTimeService.dll"]