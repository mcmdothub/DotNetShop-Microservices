using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.Database.Models;

namespace DotnetFlix.Services
{
    public interface ICartService
    {
        public Task<bool> AddToCartAsync(Guid id);
        public Task<List<Cart>> GetCartItemsAsync();
        public Task RemoveOneItemAsync(Guid id);
        public Task DeleteItemAsync(Guid id);
        public void ClearCart();
    }
}
