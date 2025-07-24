# Order-System-Solution
🧰 OrderSystem Microservices Architecture (.NET Core)

A concept-driven microservices project built with ASP.NET Core, demonstrating scalable and maintainable system design using:

✅ API Gateway with Ocelot for routing (/gateway/*)
✅ UserService and OrderService as RESTful microservices
✅ NotificationService as a background worker using RabbitMQ
✅ CloudAMQP (hosted RabbitMQ) integration via RabbitMQ.Client
✅ Clean configuration using appsettings.json and dependency injection

🧱 Services

| Service              | Description                                 | Port   |
|----------------------|---------------------------------------------|--------|
| ApiGateway           | Central routing layer via Ocelot            | 6000   |
| UserService          | Handles user-related data                   | 5010   |
| OrderService         | Manages order processing                    | 5020   |
| NotificationService  | Background consumer for notification_queue  | Worker |

📨 Messaging (RabbitMQ)

- Queue: notification_queue (durable)
- Hosted on: CloudAMQP
- Message consumption via AsyncEventingBasicConsumer
- Configured using RabbitMQSettings in appsettings.json