using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models;

namespace CRMApi.Services.Data
{
    public class ServiceData : IServiceData
    {
        private readonly CRMSystemContext _context;
        public ServiceData(CRMSystemContext context) 
        {
            _context = context;
        }

        public IEnumerable<Service> GetServices()
        {
            return _context.Services.ToList() ?? new List<Service>();
        }
        public Service GetServiceById(int id) 
        {
            return _context.Services.FirstOrDefault(s => s.Id == id) ?? throw new Exception("Услуга не найдена");
        }
        public void EditService(Service s) 
        {
            if(s.Name == null || s.Description == null) { throw new Exception("Обязательные поля не заполнены"); }
            Service service = _context.Services.FirstOrDefault(e => e.Id == s.Id) ?? throw new Exception("Услуга не найдена");
            service = s;
            _context.SaveChanges();
        }
        public void DeleteService(Service s)
        {
            _context.Services.Remove(s);
            _context.SaveChanges();
        }
        public void AddService(Service s)
        {
            _context.Services.Add(s);
            _context.SaveChanges();
        }
    }
}
