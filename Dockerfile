# Set the base image to the official .NET Core runtime image for ASP.NET Core 7
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV GOOGLE_CLIENT ""
ENV GOOGLE_SECRET ""
ENV JWT_ISSUER ""
ENV JWT_KEY "LOGIN-API"
ENV JWT_LIFETIME 86000
ENV MONGO_CLIENT_NAME "LOGIN-API"
ENV MONGODB_URI ""
ENV SECRET_PASSWORD ""

# Set the base image to the official .NET Core SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Jantzch.Server.csproj", "./"]
RUN dotnet restore "./Jantzch.Server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Jantzch.Server.csproj" -c Release -o /app/build

# Set the final image to the base image and copy the compiled application from the build image
FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Jantzch.Server.dll"]
 
