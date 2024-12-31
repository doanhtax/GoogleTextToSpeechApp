# Chỉ định image nền .NET 8.0 SDK
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Chỉ định image build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép tệp .csproj và restore dependencies
COPY ["GoogleTextToSpeechApp/GoogleTextToSpeechApp.csproj", "GoogleTextToSpeechApp/"]

# Restore dependencies (this will download packages based on .csproj)
RUN dotnet restore "GoogleTextToSpeechApp/GoogleTextToSpeechApp.csproj"

# Sao chép tất cả các tệp vào image và build project
COPY . .
WORKDIR "/src/GoogleTextToSpeechApp"
RUN dotnet build "GoogleTextToSpeechApp.csproj" -c Release -o /app/build

# Publish the app to /app/publish directory
RUN dotnet publish "GoogleTextToSpeechApp.csproj" -c Release -o /app/publish

# Đặt thư mục làm việc và chỉ định command để chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GoogleTextToSpeechApp.dll"]
