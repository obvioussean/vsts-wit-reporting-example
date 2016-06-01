# vsts-wit-reporting-example
Example project for consuming the VSTS WIT Reporting APIs

It is primarily an example of how to use the [WIT Reporting REST APIs](https://www.visualstudio.com/en-us/docs/integrate/api/wit/reporting-work-item-revisions), but also includes inserting the results into a datbase and storing them as JSON files.

REST to JSON Example:

VSTS.WIT.Reporting.exe --sourceProvider VstsRest --account https://{account}.visualstudio.com --username {username} --personalAccessToken {personalAccessToken} --targetProvider JsonFile --targetPath "C:\workItems"

JSON to SQLite Example:

VSTS.WIT.Reporting.exe --sourceProvider JsonFile --sourcePath "C:\Temp\workItems" --targetProvider SQLite --databaseFile "C:\workItems.sqlite"
