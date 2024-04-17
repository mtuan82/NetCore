ASP NET CORE WEB API:
- create project web api 
- install package Microsoft.AspNetCore.Authentication.JwtBearer
- install package Microsoft.AspNetCore.Identity.EntityFrameworkCore
- install MySql.EntityFrameworkCore
- install Microsoft.EntityFrameworkCore.Design (create DB identity and migration)
- change useMysql and connection string (specify db in appsettings.json)
If you custom identityuser then you should reapeat this step to generate identity DB
- add-migration init (create migration file)
- update-database (create scheme of identity db)
