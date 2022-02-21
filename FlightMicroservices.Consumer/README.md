# FlightMicroservices.Consumer

Possible extra features:

- Liveness and readiness probe endpoints (using `Microsoft.AspNetCore.Diagnostics.HealthChecks` and recreate project as
  WebAPI template)
- Add `Confluent.SchemaRegistry.Serdes.Avro` to use typed messages
- Validate messages from Kafka
- Expose DB connection settings via ENV