#TaskManagementAPI
This project is a Task Management API built with .NET 8.0/9.0, designed to manage tasks and comments with token-based authentication. It supports basic CRUD operations and task management functionalities, with the use of Swagger for API documentation and testing.
## 📹 Project Video Walkthrough
You can watch the project walkthrough video [here]([https://drive.google.com/your-video-link](https://drive.google.com/file/d/1fzwFH6SJqjnNMB9caYpYmh1EcwzyevLX/view?usp=sharing)).
or here
https://www.loom.com/share/41c0b94da185453c9e609a7bc1200a3f?sid=57df0eb8-fb4c-4fef-b4e7-4cc61e501326

## Assessment Task 2,3 Pdf Documentation Link [here](https://drive.google.com/file/d/11s1N3AmxlwysxY3IEPisJqtZxp4ATgxD/view?usp=sharing)

🚀 Features
User authentication with token-based security (JWT)

Task and comment management

In-memory database for data storage (no external database required)

Unit tests for API endpoints and services

Async operations for scalability

Swagger for API documentation and testing

🛠️ Requirements
Before running the project locally, ensure you have the following installed:

.NET 8.0/9.0 SDK

Postman (optional, for testing the API)

Docker (optional, for containerized deployment)

📦 Getting Started
1. Clone the Repository
bash
Copy
Edit
git clone https://github.com/Kuldip-Pre/TaskManagementAPI.git
cd TaskManagementAPI
2. Running the API Locally
bash
Copy
Edit
dotnet run
By default, the API will be available at:

https://localhost:7293/swagger/index.html

or https://localhost:5001

🧪 Testing the API with Swagger
Once the API is running, open your browser and navigate to:

bash
Copy
Edit
https://localhost:7293/swagger/index.html
Swagger provides an interactive UI for testing all available API endpoints.

🐳 Running with Docker (Optional)
1. Build the Docker image:
bash
Copy
Edit
docker build -t taskmanagementapi .
2. Run the container:
bash
Copy
Edit
docker run -p 5000:80 taskmanagementapi
The API will be accessible at http://localhost:5000.

🌱 Seed Data
Sample task and comment data is seeded into the in-memory database when the application runs for the first time. You can use Swagger or Postman to view or create data.

🔐 Authentication
The API uses JWT (JSON Web Token) for user authentication.

Login Credentials:
Username: admin

Password: admin123

Get Token:
Send a POST request to /api/auth/login with the above credentials to receive a JWT token.

Use Token:
Add the token in the Authorization header as a Bearer token:

http
Copy
Edit
Authorization: Bearer <your-jwt-token>
📘 API Documentation
View detailed documentation and test the API endpoints using Swagger UI at:

bash
Copy
Edit
https://localhost:7293/swagger/index.html
🧪 Running Unit Tests
To run the tests, execute the following command:

bash
Copy
Edit
dotnet test
📄 License
This project is licensed under the MIT License – see the LICENSE file for details.
