using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HeraDMS.Entity
{
    public partial class HeraDMSEntityClassDataContext : System.Data.Linq.DataContext
    {
        public HeraDMSEntityClassDataContext() :
            base(ConfigurationManager.ConnectionStrings["ContentDB"].ConnectionString)
        {
            OnCreated();
        }
    }
}
