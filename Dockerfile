# Sử dụng hình ảnh .NET SDK để xây dựng ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Sử dụng .NET SDK để xây dựng ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GoogleTextToSpeechApp/GoogleTextToSpeechApp.csproj", "GoogleTextToSpeechApp/"]
RUN dotnet restore "GoogleTextToSpeechApp/GoogleTextToSpeechApp.csproj"
COPY . .
WORKDIR "/src/GoogleTextToSpeechApp"
RUN dotnet build "GoogleTextToSpeechApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GoogleTextToSpeechApp.csproj" -c Release -o /app/publish

# Cấu hình để chạy ứng dụng trong môi trường sản xuất
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoogleTextToSpeechApp.dll"]
