$serviceName = "MyWindowsService"
$sourcePath = "C:\Users\User\Desktop\lecteur id\worker\MyWindowsService\bin\Release\net8.0\win-x64"
$destinationPath = "D:\ProgramService\RepService"
$serviceFile = "$destinationPath\MyWindowsService.exe"

# 1. Arrêter le service s'il existe et s'il est en cours d'execution
if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    $service = Get-Service -Name $serviceName
    if ($service.Status -eq "Running") {
        Write-Host "Arrêt du service $serviceName..."
        Stop-Service -Name $serviceName -Force

        # Attente de l'arrêt complet
        while ((Get-Service -Name $serviceName).Status -ne 'Stopped') {
            Start-Sleep -Seconds 1
        }
    }

    # Supprimer le service existant
    Write-Host "Suppression du service existant $serviceName..."
    sc.exe delete $serviceName
}

# 2. Creer le dossier de destination s'il n'existe pas
if (-not (Test-Path -Path $destinationPath)) {
    Write-Host "Creation du dossier $destinationPath..."
    New-Item -Path $destinationPath -ItemType Directory -Force | Out-Null
}

# 3. Copier les fichiers
Write-Host "Copie des fichiers vers $destinationPath..."
try {
    Copy-Item -Path "$sourcePath\*" -Destination $destinationPath -Recurse -Force
} catch {
    Write-Host "Erreur lors de la copie des fichiers : $_"
    exit 1
}

# 4. Creer un nouveau service si l'executable est trouve
if (Test-Path -Path $serviceFile) {
    Write-Host "Creation du service $serviceName..."
    
    try {
        # Creer le service avec le chemin correct
        sc.exe create $serviceName binPath= "`"$serviceFile`"" start= auto
        
        # Demarrer le service
        Write-Host "Redemarrage du service $serviceName..."
        Start-Service -Name $serviceName -ErrorAction Stop
        Write-Host "Service deploye et redemarre avec succes."
    } catch {
        Write-Host " Impossible de demarrer ou de recreer le service $serviceName : $_"
        Write-Host "Essayez de recreer le service manuellement avec 'sc create'."
    }
} else {
    Write-Host "Le fichier executable $serviceFile est introuvable."
}
