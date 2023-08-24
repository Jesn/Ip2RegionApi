FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#RUN apt-get update && apt-get install -y wget && apt-get -y install cron

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Ip2regionApi.csproj", "./"]
RUN dotnet restore "Ip2regionApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Ip2regionApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Ip2regionApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


# 添加定时任务
#ADD ./data/cronfile /etc/cron.d/cronfile
#RUN chmod 0644 /etc/cron.d/cronfile
#RUN crontab /etc/cron.d/cronfile
#CMD cron && tail -f /var/log/cron.log

ENTRYPOINT ["dotnet", "Ip2regionApi.dll"]
