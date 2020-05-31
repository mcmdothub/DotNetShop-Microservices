using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetFlix.Services
{
    public static class ApiGateway
    { 
        // API gateway
        private const string GATEWAY        = "https://localhost:44394";
                                        
        private const string PRODUCTS_API   = GATEWAY + "/products-api/";     // =  https://localhost:44304/api/products/{catchAll}
        private const string ORDERS_API     = GATEWAY + "/orders-api/";

        // Products API endpoints
        public const string API_GATEWAY_PRODUCTS        = PRODUCTS_API;
        public const string API_GATEWAY_PRODUCTS_ALL    = PRODUCTS_API + "all/";
        public const string API_GATEWAY_PRODUCTS_PAGE   = PRODUCTS_API + "page/";
        public const string API_GATEWAY_CREATE_PRODUCT  = PRODUCTS_API + "create/";
        public const string API_GATEWAY_UPDATE_PRODUCT  = PRODUCTS_API + "update/";

        // TODO: Add more orders-api endpoints
        // Orders API endpoints
        public const string API_GATEWAY_ORDERS              = ORDERS_API;
        public const string API_GATEWAY_ORDERS_ALL          = ORDERS_API + "all/";
        public const string API_GATEWAY_ORDERS_BY_USERID    = ORDERS_API + "user/";
        public const string API_GATEWAY_AGGREGATE_ORDER     = GATEWAY + "/api/generate/createorder";
    }
}
