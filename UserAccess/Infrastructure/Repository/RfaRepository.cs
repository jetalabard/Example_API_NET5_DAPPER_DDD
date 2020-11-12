using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Common.Domain.Emails;
using System.Linq;
using UserAccess.Domain.Users.Rfas;
using UserAccess.Application.Rfas;

namespace UserAccess.Infrastructure.Repository
{
    public class RfaRepository : IRfaRepository
    {
        private readonly UserAccessContext _supplierManagementContext;

        public RfaRepository(UserAccessContext supplierManagementContext)
        {
            _supplierManagementContext = supplierManagementContext;
        }

        public async Task Add(Rfa rfa)
        {
            _supplierManagementContext.Rfas.RemoveRange(_supplierManagementContext.Rfas.Where(x => x.UserId == rfa.UserId).ToList());

            await _supplierManagementContext.Rfas.AddAsync(rfa);
            await _supplierManagementContext.SaveChangesAsync();
        }

        public async Task Delete(Rfa rfa)
        {
            _supplierManagementContext.Rfas.Remove(rfa);
            await _supplierManagementContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Rfa>> GetAll()
        {
            return await _supplierManagementContext.Rfas.ToListAsync();
        }

        public async Task<Rfa> GetRfaByEmail(Email email)
        {
            return (await _supplierManagementContext.Rfas
               .ToListAsync())
               .FirstOrDefault(x => x.Email == email);
        }

        public async Task Update(Rfa rfa)
        {
            _supplierManagementContext.Rfas.Update(rfa);
            await _supplierManagementContext.SaveChangesAsync();
        }
    }
}
