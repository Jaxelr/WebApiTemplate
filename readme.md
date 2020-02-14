# WebApiTemplate

This template was created in order to facilitate the creation of a Web Api service.

## Configurations

The `appsettings.json` file includes following configurable props:

* AuthorizationServer - used to configure the Authorization server were the service will revise the token / scope information.
* ConnectrionString - used to map from a config file into the startup class the connection information needed to connect to a SQL database.

### Swagger

By default the swagger implementation follows the convention of `swagger/v1/swagger.json` for the json file. The root path contains the Swagger-UI webapp.

### HealthCheck

Usage of the Diagnostics.HealthChecks library is pointing to the `/healthcheck` url and it contains a very basic validation of uptime, that returns a json indicating the app is healthy.