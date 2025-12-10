FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TalentoPlus.Tests/TalentoPlus.Tests.csproj", "TalentoPlus.Tests/"]
COPY ["TalentoPlus.Web/TalentoPlus.Web.csproj", "TalentoPlus.Web/"]
COPY ["TalentoPlus.Api/TalentoPlus.Api.csproj", "TalentoPlus.Api/"]
RUN dotnet restore "TalentoPlus.Tests/TalentoPlus.Tests.csproj"
COPY . .
WORKDIR "/src/TalentoPlus.Tests"
ENTRYPOINT ["dotnet", "test", "--logger:trx"]
