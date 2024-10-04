using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<UserAssetDto> Assets { get; set; }
    }

    
}
