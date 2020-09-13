# downtime-alerter

Objectives
* There should a be UI which needs to be developed with ASP.NET Core MVC and
bootstrap.
* Minimal amount of UI is enough but good usability options(AJAX etc.) are bonus.
* Entities should be persisted to a database via Entity Framework.
* You are free to use any library you want for your needs.
* All unexpected errors should be logged and easily accessible.
* The application should support multiple user accounts, design it accordingly.
* The application should be as stable as possible and well tested

Summarize
* Concurrent Worker Management
* Crud processes of workers
* Flexible Notification Services
* Swagger Web Api
* Default Logging
* Unit Tests

```sh
dotnet watch run
```

Sample Create Worker
* URL :http://localhost:5000/health-check
```json 
{
  "Name":"test",
  "Url":"http://www.google.com.tr",
  "Interval":1000,
  "Email":"test@test.com",
  "NotificationType":1
}
```

Sample Create Worker with bad url
* URL :http://localhost:5000/health-check
```json 
{
  "Name":"test2",
  "Url":"http://www.google.com.t",
  "Interval":2000,
  "Email":"test@test.com",
  "NotificationType":1
}
```
 