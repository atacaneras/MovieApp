# MovieApp

MovieApp is a web application that uses The Movie Database (TMDb) API to provide information about movies, including details, related videos, similar movies, and upcoming releases.

## Features

- Search movies by keywords
- Keep track of up to 5 recent search keywords
- View movie details, including related videos and similar movies
- Browse upcoming movie releases

## Getting Started

1. Clone the repository:
```
git clone https://github.com/your-username/MovieApp.git
```
2. Navigate to the project directory:
```
cd MovieApp
```
4. Install the required dependencies:
```
dotnet restore
```
6. Configure your TMDb API Key:

- Sign up for an account at [TMDb](https://www.themoviedb.org/account/signup)
- Generate an API Key from your account settings
- Open the `appsettings.json` file and replace `YOUR_API_KEY` with your actual API Key:
  ```json
  "TMDBApiKey": "YOUR_API_KEY"
  ```
5. Build and run the application:
```
dotnet run
```
6. Open your web browser and navigate to `http://localhost:5000` to access MovieApp.

## Usage

- **Search Movies:** Enter keywords in the search bar to find movies matching your query.
- **Recent Searches:** The application keeps track of the 5 most recent search keywords.
- **Movie Details:** Click on a movie to view its details, including related videos and similar movies.
- **Upcoming Movies:** The "Upcoming Movies" section displays the latest upcoming movie releases.

## Technologies Used

- ASP.NET Core
- Razor Pages
- TMDb API

## License

This project is licensed under the [MIT License](LICENSE).
