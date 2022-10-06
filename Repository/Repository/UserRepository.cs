using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Repository.Repository
{

    public class UserRepository
    {
        private readonly TargetContext targetContext;

        public UserRepository(TargetContext targetContext)
        {
            this.targetContext = targetContext;
        }
    }
}
