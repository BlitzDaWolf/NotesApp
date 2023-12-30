# Note app

This is a simple note taking app. Using C# core 8.
The loging uses the new C# Authentication methods. This project uses tracing and metricts from OpenTelemetry.

## Using

* C# Core 8 Authentication
* OpenTelemetry
  * InfluxDB
  * Console logging
  * Jeager
* SQlite database

## Instalation

By using the given `docker-compose.yml` you can start up Jeager and InfluxDB.

After starting the services go to the [InfluxDB](http://localhost:8086/) local site. setup default user and copy the API key you get.

in the `appsettings.json` edit the `InfluxDB` section

```json
"InfluxDB": {
  "Use": true,
  "Org": "<Org-name>",
  "Bucket": "<bucket-name>",
  "Token": "<API-Token>",
  "url": "<InfluxDB-URL>"
}
```

Run the `ef database update` commaned to created the Database

Now you can start up the project.
