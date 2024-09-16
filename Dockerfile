FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV PORT=5000
EXPOSE $PORT

FROM base
WORKDIR /app
COPY sukeb/* /app
ENTRYPOINT ["dotnet", "sukeb.dll"]
