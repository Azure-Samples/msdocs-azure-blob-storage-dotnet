$LOCATION = 'eastus'       # use 'az account list-locations --output table' to list all locations
$RESOURCE_GROUP_NAME = 'rg-msdocs-blob-storage-demo'
$STORAGE_ACCOUNT_NAME = 'stblobstoragedemo123' # Replace '123' with three random numbers to get unique name  

# Create a resource group
az group create \
    --location $LOCATION \
    --name $RESOURCE_GROUP_NAME

# Create the storage account
az storage account create \
    --name $STORAGE_ACCOUNT_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --location $LOCATION

echo "This is the connection string your application will use to connect to the storage account"
echo "Safeguard this value like you would any other secret"

az storage account show-connection-string \
    -g $RESOURCE_GROUP_NAME \
    -n $STORAGE_ACCOUNT_NAME \
    --output tsv


# To remove all resources when finished use
#az group delete --name $RESOURCE_GROUP_NAME