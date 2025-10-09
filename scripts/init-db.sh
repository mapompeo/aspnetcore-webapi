#!/usr/bin/env bash
set -e

echo "Restaurando pacotes..."
dotnet restore

# Verifica dotnet-ef
if ! dotnet ef --version >/dev/null 2>&1; then
  echo "dotnet-ef não encontrado. Instale com: dotnet tool install --global dotnet-ef"
else
  echo "dotnet-ef encontrado."
fi

echo "Inicializando banco de dados..."
dotnet run --project APICatalogo -- --init-db