# Micro MQTT - Handshake API Microservice

Microservice-based handshake API using MySQL as data-layer with ACL support for micro-mqtt-broker. Used to register and handshake with new devices on-demand.

The best use for this project is to fork it and adapt to your needs, or integrate it into another service. The nature of the handshake registration is very business logic related. This is where devices may be onboarded using different access rights, or include payloads about the firmware version.

## Docker & Kubernetes

The broker was built with Kubernetes in mind, under the directory `/k8s` are sample deployment configurations.

Prebuilt images can be found at on Docker Hub at [sorena/micro-mqtt-handshake-api-microservice](https://hub.docker.com/r/sorena/micro-mqtt-handshake-api-microservice).

## Related projects

[Micro MQTT Broker](https://github.com/SorenA/micro-mqtt-broker) - Implementation of a micro MQTT broker based on Mosca MQTT with multiple authentication providers, TLS and ACL support.

[Micro MQTT Auth API Microservice](https://github.com/SorenA/micro-mqtt-auth-api-microservice) - Implementation of an API microservice that can be used with the API auth provider. Compatible with same database as this microservice.

## Configuration

Configuration is done through environment variables, in development the appsettings.Development.json file can be used.

Defaults for environment variables:

```env
ConnectionStrings__DefaultConnection=server=$(DB_HOST);port=3306;database=$(DB_DATABASE);uid=$(DB_USERNAME);password=$(DB_PASSWORD)
```

## Database

The folder `/sql` contains scripts for creating the tables with minimum needed fields, and a seed script for example data.

The passwords for the example users can be read in the seed script comments.

## Development

Copy `appsettings.Development.json.example` to  `appsettings.Development.json` and configure it to your local environment.

### Built with NuGet packages

- [BCrypt.Net-Core](https://github.com/neoKushan/BCrypt.Net-Core)
- [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
