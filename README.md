# microsoft-partner-cad-workshop
Microsoft Partner Cloud Application Development Workshop 

NOTES
-----

Download Azure CLI
https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest

**How to create my.azureauth file?**

**az** login

**az** account list --output table

**az** account set --subscription "YOUR SUBSCRIPTION ID"

**az** ad sp create-for-rbac --sdk-auth > my.azureauth

**MORE RESOURCE**

https://github.com/Azure/azure-libraries-for-net

https://azure.microsoft.com/en-us/blog/azure-management-libraries-for-net-v1-2/

https://docs.microsoft.com/en-us/dotnet/azure/dotnet-sdk-azure-concepts?view=azure-dotnet


**How to install Azure Management Library to my project**

Install-Package Microsoft.Azure.Management.Fluent -Version 1.6.0	