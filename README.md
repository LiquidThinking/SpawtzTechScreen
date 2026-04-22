## Spawtz Tech Screen

A simple multi-tenanted app for displaying the results of social sports competitions. The code is designed to vary by tenant, so that each customer can have a site which looks and feels branded and so that the competition logic can be customised.

You'll want **Visual Studio** to run it ([Community edition](https://visualstudio.microsoft.com/vs/community/) is fine), by default [LocalDb](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver17) is used (same syntax as Sql Server T-SQL) and databases are created and populated automatically. You can use [SSMS](https://learn.microsoft.com/en-us/ssms/install/install) or any T-SQL-compatible GUI to view/query the databases.

Set TechScreen.Web to startup project, or run that one directly if working with `dotnet` at the command line i.e. `cd .\TechScreen.Web\` then `dotnet run`

Please take a look at the code, get the app running and see if you can get a feel for how it works - paying particular attention to the tenant Plugin API allowing components to vary by **tenant**

We will make changes to this app in the Tech Screen process (instructions to follow)
