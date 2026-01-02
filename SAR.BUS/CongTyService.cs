using System.Collections.Generic;
using System.Linq;
using SAR.DAL.Entities;

namespace SAR.BUS
{
    public class CongTyService
    {   
        public List<CONGTY> GetAll()
        {
            using (var context = new DataContext())
            {
                return context.CONGTY.ToList();
            }
        }
    }
}