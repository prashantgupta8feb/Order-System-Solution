**Microservices Project Setup - Step-by-Step Notes**

**1. Project Initialization:**

* Created a microservices solution named `OrderSystemSolution`.
* Added multiple projects:

  * `ApiGateway` (for routing requests)
  * `UserService` (microservice for user-related operations)
  * `OrderService` (microservice for order-related operations)
  * `NotificationService` (background service for message consumption)
  * Shared project with `DTOs`.

**2. API Gateway Configuration:**

* Configured Ocelot in `ApiGateway`.
* `ocelot.json` created with the routes:

  * **UserService**:

    * `DownstreamPathTemplate`: "/api/user"
    * `UpstreamPathTemplate`: "/gateway/user"
  * **OrderService**:

    * `DownstreamPathTemplate`: "/api/order"
    * `UpstreamPathTemplate`: "/gateway/order"
  * Configured host and port mapping from gateway to each microservice.
* Verified Ocelot listens on `http://localhost:6000` (Gateway port).

**3. User Service Endpoint (Initial):**

* Created a basic `UserController` in `UserService`:

```csharp
[HttpGet]
public IActionResult GetUser()
{
    var user = new UserDto
    {
        Id = 1,
        FullName = "John Doe",
        Email = "john.doe@example.com",
        Role = "Admin"
    };
    return Ok(user);
}
```

* Verified direct access: `http://localhost:5010/api/user` works.
* Verified gateway access: `http://localhost:6000/gateway/user` works after config and routing validation.

**4. Package Installations:**

* Navigated to `UserService` directory.
* Installed packages using CLI:

  * `dotnet add package Dapper`
  * `dotnet add package System.Data.SqlClient`

(Note: Dapper not yet implemented in code)

**5. Order Service Endpoint:**

* Created `OrderController` in `OrderService`:

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrder()
    {
        return Ok("Order fetched");
    }
}
```

* Verified direct access: `http://localhost:5020/api/order` works.
* Verified gateway access: `http://localhost:6000/gateway/order` works.

**6. NotificationService & Cloud RabbitMQ Integration (Updated):**

* Created `NotificationService` project as a background worker.
* Added a class `MessageConsumer` inheriting from `BackgroundService`.
* Implemented RabbitMQ client with `AsyncEventingBasicConsumer`.
* Installed `RabbitMQ.Client v7.0.0` for compatibility.
* Switched to **CloudAMQP** hosted RabbitMQ due to local RabbitMQ issues.
* Created a queue `notification_queue` on CloudAMQP.
* Added `RabbitMQSettings` class to manage config via `appsettings.json`.
* Added configuration in `Program.cs`:

```csharp
builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ")
);
builder.Services.AddHostedService<MessageConsumer>();
```

* `appsettings.json` contains:

```json
"RabbitMQ": {
  "Uri": "amqps://<username>:<password>@<host>/<vhost>",
  "QueueName": "notification_queue"
}
```

* Fixed queue durability mismatch error by setting `durable: true` in `QueueDeclareAsync`.
* Successfully consumed message published via CloudAMQP web UI.
* Confirmed working by seeing log:

```
[NotificationService] Received message: Hello from CloudAMQP
```

**7. NotificationService Behavior:**

* Runs in terminal using `dotnet run --project NotificationService`
* Does not show Swagger – purely background worker
* Logs all activity to console
* Auto connects to CloudAMQP and listens continuously

**Next Steps:**

* ⏳ Add a sample publisher from OrderService or console app
* ⏳ Parse message body as JSON and model a DTO (e.g., NotificationMessage)
* ⏳ Add email sending logic (via Amazon SES or SMTP)
* ⏳ Optionally use Dead Letter Queue or retry handling
