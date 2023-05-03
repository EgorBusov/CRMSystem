using Microsoft.EntityFrameworkCore;

namespace CRMApi.Context
{
    public class CRMSystemContext : DbContext
    {
        public CRMSystemContext(DbContextOptions<CRMSystemContext> options)
        : base(options)
        {

        }

    }
}
