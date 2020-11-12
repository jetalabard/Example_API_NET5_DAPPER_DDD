using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Domain.Emails;
using UserAccess.Domain.Users.Rfas;

namespace UserAccess.Application.Rfas
{
    public interface IRfaRepository
    {
        Task Add(Rfa rfa);

        Task<IEnumerable<Rfa>> GetAll();

        Task Update(Rfa rfa);

        Task<Rfa> GetRfaByEmail(Email email);

        Task Delete(Rfa rfa);
    }
}
