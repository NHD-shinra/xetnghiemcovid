using System.Collections.Generic;
using System.Linq;
using SAR.DAL.Entities;

namespace SAR.BUS
{
    public class NhanVienService
    {
        public List<NHANVIEN> GetAll()
        {
            using (var context = new DataContext())
            {
                return context.NHANVIEN.Include("CONGTY").ToList();
            }
        }

        public NHANVIEN FindById(string id)
        {
            using (var context = new DataContext())
            {
                return context.NHANVIEN.FirstOrDefault(p => p.ID == id);
            }
        }

        public void InsertUpdate(NHANVIEN nv)
        {
            using (var context = new DataContext())
            {
                var existingNV = context.NHANVIEN.FirstOrDefault(p => p.ID == nv.ID);
                
                if (existingNV == null)
                {
                    context.NHANVIEN.Add(nv);
                }
                else
                {
                    existingNV.HoTen = nv.HoTen;
                    existingNV.SoLanXN = nv.SoLanXN;
                    existingNV.AmTinh = nv.AmTinh;
                    existingNV.MaCty = nv.MaCty;
                }
                context.SaveChanges();
            }
        }

        public List<NHANVIEN> GetDSDuongTinh()
        {
            using (var context = new DataContext())
            {
                return context.NHANVIEN.Include("CONGTY")
                    .Where(p => p.AmTinh == false).ToList();
            }
        }

        public List<string> GetCongTyDaTestDuYC()
        {
            using (var context = new DataContext())
            {
                var result = from ct in context.CONGTY
                            join nv in context.NHANVIEN on ct.MaCty equals nv.MaCty into nvGroup
                            let soNVDaTest = nvGroup.Count()
                            where soNVDaTest >= ct.SLNV
                            orderby ct.TenCty
                            select ct.TenCty;
                            
                return result.ToList();
            }
        }
    }
}