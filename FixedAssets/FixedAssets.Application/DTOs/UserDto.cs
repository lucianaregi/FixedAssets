using System.Collections.Generic;

namespace FixedAssets.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; } 
        public decimal Balance { get; set; }
        public List<UserAssetDto> Assets { get; set; }
        public List<OrderDto> Orders { get; set; } 
    }
}
