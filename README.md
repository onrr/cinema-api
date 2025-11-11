---
# Cinema-API

Cinema API is a project, it gives information about movies, show times and cinema theaters. People can see a list of movies, read details about a movie, theater and check show times.

---
## Technologies

This project uses these technologies:

- **.NET Core 9** - The main framework of the project.
- **SQLite** - The database.
- **EntityFrameworkCore** - The ORM for working with the database.
- **JWT** - For user authentication.
- **Identity** - For user management and register/login.
---

## Environment Variables
This project uses environment settings for security and database.
All setting are in the **appsettings** json file.

- **Jwt**: This is for user login and security
    - **Key**: A secre key for creating tokens. (it must be 32 characters long)
    - **Issuer**: who makes the tokens.
    - **Audience**: Who can use the token.
- **ConnectionStrings**: This if for the database.
    - **DefaultConnection**: The database file for this project.
---

## How to Start the Project

1. Make sure you have **.NET Core 9 SDK** installed on your computer.
2. Open the project in your favorite IDE (like Visual Studio or VS Code).
3. Open a terminal and go to the project folder.
4. Run the command:
```bash
dotnet run
```
5. The project uses the host address from cinema.http 
The variable is:
```bash
@cinema_HostAddress = http://localhost:5287
```

You can change this address if you want.

6. Now the project is running.

- You can use the Cinema API. See the examples below to know how to use it.
---

## API Usage

### ***AuthController***

- **Route:** `/api/auth`  
- **Access:** Some methods need login, some do not

#### 1. Register a new user

- **Method:** `POST`  
- **URL:** `/api/auth/register`  
- **Description:** Create a new user account.  
- **Access:** Public (no login needed)

#### 2. Login

- **Method:** `POST`  
- **URL:** `/api/auth/login`  
- **Description:** Login with your account.  
- **Access:** Public (no login needed)

#### 3. Logout

- **Method:** `POST`  
- **URL:** `/api/auth/logout`  
- **Description:** Logout the current user.  
- **Access:** User must be logged in

#### 4. Get current user (me)

- **Method:** `GET`  
- **URL:** `/api/auth/me`  
- **Description:** Get information about the logged-in user.  
- **Access:** User must be logged in

### ***MovieController***

- **Route:** `/api/movie`  
- **Access:** Admin only

#### 1. Get all movies

- **Method:** `GET`  
- **URL:** `/api/movie`  
- **Description:** Get a list of all movies.

#### 2. Get a movie by ID

- **Method:** `GET`  
- **URL:** `/api/movie/{id}`  
- **Example:** `/api/movie/1`  
- **Description:** Get the details of the movie with the given ID.

#### 3. Create a new movie

- **Method:** `POST`  
- **URL:** `/api/movie`  
- **Description:** Add a new movie.

#### 4. Update a movie

- **Method:** `PUT`  
- **URL:** `/api/movie/{id}`  
- **Example:** `/api/movie/1`  
- **Description:** Update the movie with the given ID.

#### 5. Delete a movie

- **Method:** `DELETE`  
- **URL:** `/api/movie/{id}`  
- **Example:** `/api/movie/1`  
- **Description:** Delete the movie with the given ID.

### ***TheaterController***

- **Route:** `/api/theater`  
- **Access:** Some methods are public, some are Admin only

#### 1. Get all theaters

- **Method:** `GET`  
- **URL:** `/api/theater`  
- **Description:** Get a list of all theaters.  
- **Access:** Public (everyone can use)

#### 2. Get a theater by ID

- **Method:** `GET`  
- **URL:** `/api/theater/{id}`  
- **Example:** `/api/theater/1`  
- **Description:** Get the details of the theater with the given ID.  
- **Access:** Public (everyone can use)

#### 3. Create a new theater

- **Method:** `POST`  
- **URL:** `/api/theater`  
- **Description:** Add a new theater.  
- **Access:** Admin only

#### 4. Update a theater

- **Method:** `PUT`  
- **URL:** `/api/theater/{id}`  
- **Example:** `/api/theater/1`  
- **Description:** Update the theater with the given ID.  
- **Access:** Admin only

#### 5. Delete a theater

- **Method:** `DELETE`  
- **URL:** `/api/theater/{id}`  
- **Example:** `/api/theater/1`  
- **Description:** Delete the theater with the given ID.  
- **Access:** Admin only

### ***ShowtimeController***

- **Route:** `/api/showtime`  
- **Access:** Some methods are public, some are Admin only

#### 1. Get all showtimes

- **Method:** `GET`  
- **URL:** `/api/showtime`  
- **Description:** Get a list of all showtimes.  
- **Access:** Public (everyone can use)

#### 2. Get a showtime by ID

- **Method:** `GET`  
- **URL:** `/api/showtime/{id}`  
- **Example:** `/api/showtime/1`  
- **Description:** Get the details of the showtime with the given ID.  
- **Access:** Public (everyone can use)

#### 3. Create a new showtime

- **Method:** `POST`  
- **URL:** `/api/showtime`  
- **Description:** Add a new showtime.  
- **Access:** Admin only

#### 4. Update a showtime

- **Method:** `PUT`  
- **URL:** `/api/showtime/{id}`  
- **Example:** `/api/showtime/1`  
- **Description:** Update the showtime with the given ID.  
- **Access:** Admin only

#### 5. Delete a showtime

- **Method:** `DELETE`  
- **URL:** `/api/showtime/{id}`  
- **Example:** `/api/showtime/1`  
- **Description:** Delete the showtime with the given ID.  
- **Access:** Admin only

### ***ReservationController***

- **Route:** `/api/reservation`  
- **Access:** User must be logged in

#### 1. Get all reservations of the logged-in user

- **Method:** `GET`  
- **URL:** `/api/reservation`  
- **Description:** Get all reservations of the logged-in user.

#### 2. Get a reservation by ID

- **Method:** `GET`  
- **URL:** `/api/reservation/{id}`  
- **Example:** `/api/reservation/1`  
- **Description:** Get the details of the reservation with the given ID.

#### 3. Create a new reservation

- **Method:** `POST`  
- **URL:** `/api/reservation`  
- **Description:** Create a new reservation.

#### 4. Delete a reservation

- **Method:** `DELETE`  
- **URL:** `/api/reservation/{id}`  
- **Example:** `/api/reservation/1`  
- **Description:** Delete the reservation with the given ID.

### ***AdminController***

- **Route:** `/api/admin`  
- **Access:** Admin only (read-only)

#### 1. Get all theaters

- **Method:** `GET`  
- **URL:** `/api/admin/theaters`  
- **Description:** Admin can see all theaters.

#### 2. Get a theater by ID

- **Method:** `GET`  
- **URL:** `/api/admin/theater/{id}`  
- **Example:** `/api/admin/theater/1`  
- **Description:** Admin can see the details of a theater by ID.

#### 3. Get all showtimes

- **Method:** `GET`  
- **URL:** `/api/admin/showtimes`  
- **Description:** Admin can see all showtimes.  
- **Access:** Admin only

#### 4. Get a showtime by ID

- **Method:** `GET`  
- **URL:** `/api/admin/showtime/{id}`  
- **Example:** `/api/admin/showtime/1`  
- **Description:** Admin can see the details of a showtime by ID.

#### 5. Get all reservations

- **Method:** `GET`  
- **URL:** `/api/admin/reservations`  
- **Description:** Admin can see all reservations.

#### 6. Get a reservation by ID

- **Method:** `GET`  
- **URL:** `/api/admin/reservation/{id}`  
- **Example:** `/api/admin/reservation/1`  
- **Description:** Admin can see the details of a reservation by ID.

#### 7. Delete a reservation by ID

- **Method:** `DELETE`  
- **URL:** `/api/admin/cancelReservation/{id}`  
- **Example:** `/api/admin/cancelReservation/1`  
- **Description:** Admin can delete any reservation by ID.

