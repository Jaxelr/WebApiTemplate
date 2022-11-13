# WebApiTemplate

[![Build status](https://ci.appveyor.com/api/projects/status/x201qsq5f3lwfll9?svg=true)](https://ci.appveyor.com/project/Jaxelr/webapitemplate) ![MyGet](https://img.shields.io/myget/webapitemplate/v/WebApiTemplate?style=flat)

This template was created in order to facilitate the creation of a dotnet WebApi services, using common dependencies outside of the basic dotnet new webapi template.

## Installation

Make sure the dotnetcore sdk is installed by running `dotnet --version` on a console.

For installation of latest version, run:

`dotnet new -i  "WebApiTemplate::*" --nuget-source https://www.myget.org/F/webapitemplate/api/v3/index.json`

Currently it lives on the myget source only.

### Previous versions

For templates targeting previous versions of dotnet use the version number instead of `*` on the installation command:

| dotnet version | template version |
| -- | -- |
| 7.0 | latest |
| 6.0 | 1.3.19 |
| 5.0 | 1.2.2 |
| 3.1 | 1.0.16 |

## Usage

For usage, run:

`dotnet new wbapi -o ProjectName`

Where Projectname is the name given to the api solution.

## Configurations

The `appsettings.json` file includes following configurable props:

* AuthorizationServer - used to configure the Authorization server were the service will revise the token / scope information. 
* ConnectrionString - used to map from a config file into the startup class the connection information needed to connect to a SQL database.

### Swagger

By default the swagger implementation follows the convention of `swagger/v1/swagger.json` for the json file. The root path contains the Swagger-UI webapp.

### HealthCheck

Usage of the Diagnostics.HealthChecks library is pointing to the `/health` url and it contains a very basic validation of uptime, that returns a json indicating the app is healthy.

### Uninstallation

`dotnet new -u WebApiTemplate`

### Dependencies

The following nuget libraries are included:

- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Dapper](https://github.com/StackExchange/Dapper)
- [AspNetCore.Diagnostics.HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)