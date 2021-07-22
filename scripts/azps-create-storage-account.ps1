$location = 'eastus'   # Use 'Get-AzLocation | Select-Object -Property DisplayName,Location' to list all locations
$resourceGroupName = 'rg-msdocs-blob-storage-demo'
$storageAccountName = 'stblobstoragedemo123'   # Replace 123 with three random numbers to get unique name

# Create a resource group
New-AzResourceGroup `
    -Location $location `
    -Name $resourceGroupName

# Create the storage account
New-AzStorageAccount `
    -Name $storageAccountName `
    -ResourceGroupName $resourceGroupName `
    -Location $location `
    -SkuName Standard_LRS

# Get Connection String for Storage Account
$storageKey=(Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageAccountName).Value[0]
$storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=$storageAccountName;AccountKey=$storageKey;EndpointSuffix=core.windows.net"

Write-Host 'This is the connection string your application will use to connect to the storage account'
Write-Host 'Safeguard this value like you would any other secret'
Write-Host $storageConnectionString


# To remove all resources when finished use
#Remove-AzResourceGroup -Name $resourceGroupName    