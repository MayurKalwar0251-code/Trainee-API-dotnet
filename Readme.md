
# Trainee Management System

A Trainee management system API to manage all trainee records by performing CRUD operations through REST APIs. Application is made in Asp.NET and the database is EF Core In-Memory.

## Tech Stack

ASP.NET, OpenAPI / Swagger, EF Core


## How to Run

Go to the project directory. First install all required packages.
```bash
  dotnet restore
```
To launch the project in development.
```bash
 dotnet run --launch-profile https    
```
To launch in watch mode.
```bash
  dotnet watch --launch-profile https    
```




## API Reference

#### Health check of application

```http
  GET /api/Health
```

#### Interactive Swagger UI for testing of routes
```http
  GET /swagger
```

#### Get all Trainees with optional search query 

```http
  GET /api/trainee/all
```

| query | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `search` | `string` | **Optional** It checks whether first name, last name, tech stack or email contains search string.  |

#### Get Trainee by Id

```http
  GET /api/trainee/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `long` | **Required**. Id of trainee to fetch |

#### Add Trainee 
```http
  POST /api/trainee
```
Request Body
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `firstName`      | `string` | **Required**. First name min 3 max 50. |
| `lastName`      | `string` | **Required**. Last name min 3 max 50 |
| `email`      | `string` | **Required**. Valid email. |
| `techStack`      | `string` | **Required**. |
| `status`      | `string` | **Required**. status in 'Active', 'Inactive','Completed' |

#### Update Trainee 
```http
  PUT /api/trainee/${Id}
```
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `long` | **Required**. Id of trainee to update |

Request Body
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `firstName`      | `string` | **Required**. First name min 3 max 50. |
| `lastName`      | `string` | **Required**. Last name min 3 max 50 |
| `email`      | `string` | **Required**. Valid email. |
| `techStack`      | `string` | **Required**. |
| `status`      | `string` | **Required**. status in 'Active', 'Inactive','Completed' |

#### Delete Trainee 
```http
  DELETE /api/trainee/${Id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `long` | **Required**. Id of trainee to delete |
## Sample Request JSON

```bash
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "techStack": "string",
  "status": "string"
}
```

## Sample Response JSON

```bash
{
  "status": "bool",
  "message": "string",
  "data"?: "T",
  "error"?: "object"
}
```

## Known limitations

The database is stored in In Memory, once the application restarts data gets lost. The api lacks security for authentication and authorisation purpose & Error Handling.