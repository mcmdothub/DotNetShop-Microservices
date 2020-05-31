* DotnetFlix (MVC WEB)
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
- Microsoft.AspNetCore.Identity.UI


* DotnetFlix.OrderService
 NuGet packages:
 -  Microsoft.EntityFrameworkCore.Sqlite
 -  Microsoft.EntityFrameworkCore.SqlServer
 -  Microsoft.EntityFrameworkCore.Tools
 -  Microsoft.VisualStudio.Web.CodeGeneration.Design



* Repositories
- IOrdersRepository
- OrdersRepository : IOrdersRepository



* DotnetFlix.ApiGateway
- base-url : https://localhost:44328
      - DotnetFlix.ProductService (launchsettings.json)
            products -endpoint - localhost:4433//api/products/{catchAll}
      - DotnetFlix.OrderService   (launchsettings.json)
            orders   -endpoint - localhost:4433//api/orders/{catchAll}

      - Products -endpoint : {base-url}/products-api/{catchAll}
      - Orders   -endpoint : {base-url}/orders-api/{catchAll}
