# Shopping.NET

Small e-shopping app made with C# .NET and Vue 3.

# Install

To install the project, first, run the following commands :

```
git clone https://github.com/Cedricsol/shopping.NET.git
cd shopping.NET
```

To connect the backend to your database, go to appsettings.json and modify this line :

```
"DefaultConnection": "Server=localhost;Database=ShoppingDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

Then you need to update the migration with the following command :

```
dotnet ef database update
```

To install the frontend, run the following commands :

```
cd frontend
npm install
```

# Run

To run the backend, open a terminal and run the following commands :

```
cd Shopping.NET
dotnet run
```

To run the frontend, open a new terminal and run the following commands :

```
cd frontend
npm run dev
```

# Images

When adding a product, you can add an image. Images need to be placed under the "frontend/public/images" directory. And you need to put "/images/file-name.extension" in the input form.

# Tests

To run the backend tests, run the following commands from the root directory : 
```
cd Shopping.NET.Tests
dotnet test
```

To run the frontend tests, run the following command from the frontend directory :
```
npm run test:unit
```


# Technologies Used

- ASP.NET Core
- Entity Framework Core
- SQL Server
- Vue 3

# Remaining work to do

- Add tests
- Add authentication and only admins can add products
- Add a cart
- Image Uploading
