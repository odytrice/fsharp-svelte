FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install NodeJS
ENV NODE_VERSION=22.4.0
RUN apt install -y curl
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash
ENV NVM_DIR=/root/.nvm
RUN . "$NVM_DIR/nvm.sh" && nvm install ${NODE_VERSION}
RUN . "$NVM_DIR/nvm.sh" && nvm use v${NODE_VERSION}
RUN . "$NVM_DIR/nvm.sh" && nvm alias default v${NODE_VERSION}
ENV PATH="/root/.nvm/versions/node/v${NODE_VERSION}/bin/:${PATH}"
RUN node --version
RUN npm --version

WORKDIR /root/build/

# Restore dotnet cli tools
COPY .config/ .config/
RUN dotnet tool restore

# Dotnet Package Restore
WORKDIR /root/build/Server
COPY ./src/Server/Server.fsproj .
RUN dotnet restore

# NPM Restore
WORKDIR /root/build/Client
COPY ./src/Client/package*.json .
RUN npm ci

WORKDIR /root/build
# Copy Application
COPY . .

# Build
RUN dotnet build

# Bundle
RUN dotnet fsi build.fsx -t bundle

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /root/app/

COPY --from=build /root/build/.deploy ./

EXPOSE 80/tcp
CMD ["dotnet", "Server.dll"]