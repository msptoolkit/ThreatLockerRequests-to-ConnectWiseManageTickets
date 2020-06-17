# ThreatLocker to Manage Integration

## Installation Instructions

- Download and Install MySQL Server Community Edition (make not of Root password during installation)
- Run the Start.SQL script against the MySQL server.
- Clone the repository
- Adjust the appsettings.json file to include the IP and Root password of the MySQL server
- Run dotnet publish -r win-x64 -c Release --self-contained 
- There will be a Publish directory containing the executable
  - C:\source\threatlocker\TLManageService\bin\Release\netcoreapp3.1\win-x64\publish\

- Create the service by running the following command from an elevated command promot: 
  - SC CREATE "TLManageService" binpath=C:\source\threatlocker\TLManageService\bin\Release\netcoreapp3.1\win-x64\publish\TLService.exe
- Open a browser and navigate to http://localhost:5000
