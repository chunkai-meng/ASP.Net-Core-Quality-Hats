# **THe order of MVC is:**
1. Add a model
2. Modify the action method in the controller
3. add a view
4. Seed data to AccountVewModels

    `PM> Update-Database` or  
    `dotnet ef database update -c ApplicationDbContext`

5. After change Model we need to migrate and update DB schema
```
~ dotnet ef migrations add addAddressToUser -c ApplicationDbContext
~ dotnet ef database update -c ApplicationDbContext
# or in PM console:
Add-Migration Address -Context ApplicationDbContext
Update-Database -Context ApplicationDbContext
```
To undo this action:
`dotnet ef migrations remove -c ApplicationDbContext`

### Part Four: Implementing Admin Control over Member users.
- Add a new property `Enabled` to ApplicationUser Model
- `Add-Migration Enabled -Context ApplicationDbContext`
- Make sure the `defaultValue: true` in new migration file
- `Update-Database -Context ApplicationDbContext`
- Add Enabled=true to new registered user by default
- Add Enabled checking in Login action
- Add Enabled = true for Admin Account by defualt in Startup.cs (Data Seed)
**Show Member**
- Create a new Controller with View via Scaffolding
    - Model: ApplicationUser
    - Data: ApplicationsDbContext
- Set AdminApplicationUsersController class ot [Authorize(Roles = "Admin")]
- Add using dependency
- Add a ReturnAllMembers method to Controller
- Change Index action to show only members except admin
- Change _Layout to show Member Link in when Admin login
- Change Index View to show the item you need
- Add Enable/Disable asp-action link to member view
- Add relevant method in AdminApplicationUsersController.cs
- Some Change

## Problems Summary:
if see `The term 'add-migration' is not recognized as the name of a cmdlet` error.
- close VS2015 or even restart computer. then open the project should work.
    - Or cmd to /Project Name/src/Project Name/
    - use `dotnet ef migrations add addAddressToUser -c ApplicationDbContext`
