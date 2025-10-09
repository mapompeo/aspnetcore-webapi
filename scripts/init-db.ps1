# Script PowerShell para inicializar o banco de dados
Write-Host "Restaurando pacotes..."
dotnet restore

# Verifica se o dotnet-ef est� instalado
try {
    dotnet ef --version > $null 2>&1
} catch {
    Write-Host "dotnet-ef n�o encontrado, instalando ferramenta global..."
    dotnet tool install --global dotnet-ef
}

# Executa a inicializa��o do banco
Write-Host "Inicializando banco de dados..."
dotnet run --project APICatalogo -- --init-db