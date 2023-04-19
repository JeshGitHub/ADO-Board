# ADO-Board

---
Technologies:
- angular
- asp.net core web api
- csharp
- azure

Description: This application implements the following
- Authenticates with Azure DevOps
- Query Work Items based on a Tag value using WIQL
- Traverses the Parent-Child relationship until there are no more children
- Prints out Item Id and Title for each level

---

# Steps to run
- ASP.NET Core Web API - F5- AzureDevOps\ADO\ADO.sln
- Angular SPA - ADOUI\ado-board-ui
  - npm install
  - ng serve --open
  
