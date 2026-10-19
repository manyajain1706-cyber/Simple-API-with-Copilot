# Simple API with Copilot

A beginner-friendly ASP.NET Core Minimal API demonstrating CRUD endpoints, input validation, and custom request-logging middleware.

## Requirements

- .NET 8 SDK or newer with .NET 8 targeting support
- A code editor such as Visual Studio or Visual Studio Code
- Git and a GitHub account for submission

## Run the API

Open a terminal in the folder containing `SimpleApiWithCopilot.csproj`, then run:

```bash
dotnet restore
dotnet run
```

The terminal prints the local HTTP/HTTPS address. Open the root address (`/`) to see a short API status message. Use the same base address for the endpoints below.

> This starter stores records in memory, so newly added or changed data resets when the application stops.

## API endpoints

| Method | Endpoint | Purpose | Successful response |
|---|---|---|---|
| GET | `/api/tasks` | List all tasks | `200 OK` |
| GET | `/api/tasks/{id}` | Get one task | `200 OK` |
| POST | `/api/tasks` | Create a task | `201 Created` |
| PUT | `/api/tasks/{id}` | Update a task | `200 OK` |
| DELETE | `/api/tasks/{id}` | Delete a task | `204 No Content` |

Unknown IDs return `404 Not Found`. Invalid input returns `400 Bad Request` with validation details.

## Example request body

```json
{
  "title": "Finish API assignment",
  "description": "Test each CRUD endpoint",
  "isCompleted": false
}
```

The title is required and must contain 3–80 characters. The description can contain up to 300 characters.

## Test with curl

Replace `http://localhost:5000` with the address printed by `dotnet run`.

```bash
curl http://localhost:5000/api/tasks

curl -X POST http://localhost:5000/api/tasks \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Practice API\",\"description\":\"Try POST\",\"isCompleted\":false}"

curl -X PUT http://localhost:5000/api/tasks/1 \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Updated task\",\"description\":\"Try PUT\",\"isCompleted\":true}"

curl -i -X DELETE http://localhost:5000/api/tasks/1
```

To test validation, send a POST request with a title shorter than 3 characters or omit the title.

## How the project addresses the rubric

- **CRUD endpoints:** GET all, GET by ID, POST, PUT, and DELETE.
- **Input validation:** Data annotations plus validation responses for invalid request data.
- **Middleware:** `RequestLoggingMiddleware` logs HTTP method, path, response status, and elapsed time.
- **Error handling:** Missing IDs return a clear 404 response.
- **Organization:** Models and middleware are kept in separate folders.

## Copilot development summary — personalize before submitting

Use the following as a draft and edit it so it accurately describes what you personally did with GitHub Copilot:

1. **Planning and endpoint creation:** I used Copilot to help outline the CRUD routes and explain the purpose of each HTTP method. I reviewed the generated suggestions and checked that the route names and response codes matched the requirements.
2. **Validation:** I asked Copilot for suggestions on validating incoming task data. I reviewed the required-title and length rules and tested invalid input to confirm that the API returns validation errors.
3. **Middleware:** I used Copilot to understand how ASP.NET Core middleware fits into the request pipeline and to draft request logging. I checked that the log includes the request method, path, status code, and elapsed time.
4. **Testing and debugging:** I used Copilot as a coding assistant while checking the endpoints, corrected any issues found during testing, and verified the expected success and error responses.

**Important:** Do not claim you used Copilot for a step unless you actually did. If your course requires evidence of Copilot usage, include your real prompts, suggestions, and changes.

## Before submission

- Run `dotnet restore` and `dotnet run`.
- Test all five endpoints and one invalid request.
- Update this README's Copilot summary to reflect your actual workflow.
- Push the project files (not the ZIP alone) to a **public GitHub repository**, unless your course says otherwise.
- Submit the repository URL in the course form.
