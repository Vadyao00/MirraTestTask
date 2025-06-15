# Test workflow steps locally
Write-Host "Testing workflow steps..."

# Debug - List directory structure
Write-Host "`nDebug - List directory structure"
Get-Location
Write-Host "Current directory contents:"
Get-ChildItem
Write-Host "Backend directory contents:"
Get-ChildItem backend/
Write-Host "MirraApi directory contents:"
Get-ChildItem backend/MirraApi/

# Restore .NET dependencies
Write-Host "`nRestore .NET dependencies"
Set-Location backend/MirraApi
Get-Location
Get-ChildItem
dotnet restore
Set-Location ../..

# Build .NET
Write-Host "`nBuild .NET"
Set-Location backend/MirraApi
dotnet build --no-restore
Set-Location ../..

# Test .NET
Write-Host "`nTest .NET"
Set-Location backend/MirraApi
dotnet test --no-build --verbosity normal
Set-Location ../..

# Install Frontend dependencies
Write-Host "`nInstall Frontend dependencies"
Set-Location frontend/MirraFront
npm install
Set-Location ../..

# Build Frontend
Write-Host "`nBuild Frontend"
Set-Location frontend/MirraFront
npm run build
Set-Location ../..

Write-Host "`nWorkflow test completed!" 