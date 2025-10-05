FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/TaskManager.Domain/TaskManager.Domain.csproj", "src/TaskManager.Domain/"]
COPY ["src/TaskManager.Application/TaskManager.Application.csproj", "src/TaskManager.Application/"]
COPY ["src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj", "src/TaskManager.Infrastructure/"]
COPY ["src/TaskManager.API/TaskManager.API.csproj", "src/TaskManager.API/"]

# Restore dependencies
RUN dotnet restore "src/TaskManager.API/TaskManager.API.csproj"

# Copy source code
COPY . .
WORKDIR "/src/src/TaskManager.API"
RUN dotnet build "TaskManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManager.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskManager.API.dll"]