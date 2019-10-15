# Micro MQTT - Handshake API Microservice

Microservice-based handshake API using MySQL as data-layer with ACL support for micro-mqtt-broker. Used to register and handshake with new devices on-demand.

**The best use for this project is to fork it and adapt to your needs, or integrate it into another service. The nature of the handshake and registration is very business specific.
The default implementation here grants full access to every new device.**

This could be where devices may be onboarded using different access rights, or include payloads about the firmware version.

An extension could be different onboarding tokens for different access types and devices.

Another feature could be key rotation on handshake.

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
OnboardingToken=test-token
```

If no OnboardingToken is present, it will not be required from the registring devices in the requests, allowing open access to registration of new devices.

## Database

The folder `/sql` contains scripts for creating the tables with minimum needed fields, and a seed script for example data.

The passwords for the example users can be read in the seed script comments.

## Registering a device

A new device can be onboarded on-demand using the `/register` endpoint. The onboarding token is only required if the service is configured with it.

```http
POST /register
{
    "onboardingToken": "foo"
}
```

The response will then look like the following, given the registration was successful.

```json
{
    "isSuccessful": true,
    "identity": "id-y8GxLk62xqMpNLltmUvNqRm5iTK472D4SrtQUnaM7a43nll8nBmUvh6gWLP1Z",
    "username": "un-C45nSWgcVZ93iPQXsS7sMQ6PsJvvM",
    "password": "vyIUJtGsbDThObamx4Ssj0QyKPvOpgum"
}
```

The `identity` should be saved as a secret on the device, it's used to handshake and should not be shared.

The `username` and `password` fields are the values used to sign in to related services, like the [Micro MQTT Broker](https://github.com/SorenA/micro-mqtt-broker).

The default ACL for newly onboarded devices is full access.

## Handshaking a device

Handshakes can be completed using the `/` endpoint, the current handshake doesn't do anything but log the handshake to the database.

```http
POST /
{
    "identity": "id-y8GxLk62xqMpNLltmUvNqRm5iTK472D4SrtQUnaM7a43nll8nBmUvh6gWLP1Z"
}
```

The response will then look like the following, given the handshake was successful.

```json
{
    "isSuccessful": true
}
```

In specific implementations the handshake could be used to register firmware versions, regenerate identity token or rotate login from registration.

## Development

Copy `appsettings.Development.json.example` to  `appsettings.Development.json` and configure it to your local environment.

### Built with NuGet packages

- [BCrypt.Net-Core](https://github.com/neoKushan/BCrypt.Net-Core)
- [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
