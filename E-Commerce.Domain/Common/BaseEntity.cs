using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Common
{
    public abstract class BaseEntity<TKey>
    {
        //Tkey -- generic becuase it's not always int 
        public TKey Id { get; set; } = default!;
    }
}
