# FlightMicroservices.WebApi

Possible extra features:
- Liveness and readiness probe endpoints (using `Microsoft.AspNetCore.Diagnostics.HealthChecks`)
- Model validation filter suppression and write validation errors to logs (now errors returns only in HTTP response)
- Versioned controllers and models separation (`Controllers/V1/...` and `Models/V1/...`)
- `FlightUpdateDto` validation: use regexp to validate flight number string
- Add `Confluent.SchemaRegistry.Serdes.Avro` to use typed messages

Notes:
- Use JSON serialization attributes to support non-standard properties naming or override global serialization options (snake case, camel case, whatever)
