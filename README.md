# Description
This is API project for my college project. To check GUI go to https://github.com/gtteamamxx/PsychoCare.Client

# Requirments

1. MS SQL Database
2. .NET Core 3.1

# Installation
Simple clone repository and change connection string in `PsychoCareContextInitializer.cs` file.
for example: 
```
public static string ConnectionString = "Data Source = database_url; Initial Catalog = database; persist security info=True ; user id = uid; password=pwd ; MultipleActiveResultSets=True";
```

# Running API
Type in terminal
`dotnet run --project "PsychoCare.API"`

# Testing
Type in terminal
`dotnet test`
