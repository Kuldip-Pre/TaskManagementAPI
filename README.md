# TaskManagementAPI
This project is a Task Management API built with .NET 8.0/9.0, designed to manage tasks and comments with token-based authentication. It supports basic CRUD operations and task management functionalities, with the use of Swagger for API documentation and testing.

Features
User authentication with token-based security (JWT)

Task and comment management

In-memory database for data storage (no external database required)

Unit tests for API endpoints and services

Async operations for scalability

Swagger for API documentation and testing

Requirements
Before running the project locally, ensure you have the following installed:

.NET 8.0/9.0 SDK or later

Postman (optional, for testing the API)

Docker (optional, for containerized deployment)

Getting Started
1. Clone the Repository
Clone the repository to your local machine:

bash
Copy
Edit
git clone https://github.com/Kuldip-Pre/TaskManagementAPI.git
cd TaskManagementAPI
2. Running the API Locally
To run the API locally, use the following command:

bash
Copy
Edit
dotnet run
By default, the API will be available at https://localhost:7293/swagger/index.html or https://localhost:5001 (for HTTPS).

3. Testing the API with Swagger
Once the API is running, you can test and explore the API endpoints using Swagger UI. Open your browser and navigate to:

bash
Copy
Edit
https://localhost:7293/swagger/index.html
Swagger provides an interactive UI for testing all available API endpoints.

4. Running with Docker (Optional)
If you prefer to run the API with Docker:

Build the Docker image:

bash
Copy
Edit
docker build -t taskmanagementapi .
Run the container:

bash
Copy
Edit
docker run -p 5000:80 taskmanagementapi
The API will be accessible at http://localhost:5000.

5. Seed Data
Sample task and comment data is seeded into the in-memory database when the application runs for the first time. You can find sample seed data in the database or via API requests to create tasks and comments.

Authentication
The API uses JWT (JSON Web Token) for user authentication.

Send a POST request to /api/auth/login with your username - admin and password -admin123 to receive a JWT token.

Use the JWT token in the Authorization header as a Bearer token for further requests:

bash
Copy
Edit
Authorization: Bearer <your-jwt-token>
API Documentation
Once the API is running, you can view the detailed documentation and test the API endpoints using the Swagger UI at:

bash
Copy
Edit
https://localhost:7293/swagger/index.html
This provides an interactive UI for testing all available endpoints.

Running Unit Tests
The project includes unit tests that validate the functionality of the API and its services. To run the tests, execute the following command:

bash
Copy
Edit
dotnet test
License
This project is licensed under the MIT License - see the LICENSE file for details.
