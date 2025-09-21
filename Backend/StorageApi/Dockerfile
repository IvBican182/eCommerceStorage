# -------------------------------
# Stage 1: Build
# -------------------------------
# Koristimo .NET SDK 8 za build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Postavljamo radni direktorij unutar containera
WORKDIR /app

# Kopiramo samo projektni file i restore-amo NuGet pakete
COPY *.csproj ./
RUN dotnet restore

# Kopiramo ostatak koda i buildamo/publishamo aplikaciju u folder 'out'
COPY . ./
RUN dotnet publish -c Release -o out

# -------------------------------
# Stage 2: Runtime
# -------------------------------
# Koristimo manji runtime image samo za pokretanje aplikacije
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Radni direktorij unutar runtime containera
WORKDIR /app

# Kopiramo build iz prvog stage-a
COPY --from=build /app/out ./

# Dopuštamo da container sluša na portu 5000
EXPOSE 5000

# ASP.NET server će slušati na svim mrežnim interface-ima
ENV ASPNETCORE_URLS=http://+:5000

# Komanda koja se izvršava kad container starta
ENTRYPOINT ["dotnet", "StorageApi.dll"]