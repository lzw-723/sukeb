FROM mcr.microsoft.com/dotnet/aspnet:8.0

USER $APP_UID
ENV PORT=5000
EXPOSE $PORT

COPY sukeb /app
WORKDIR /app
ENTRYPOINT ["dotnet", "sukeb.dll"]
