using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class SetSecretEvent
    {
        public SecretKey Key { get; set; }
        public string Value { get; set; }
    }

    public class RemoveSecretEvent
    {
        public SecretKey Key { get; set; }
    }

    public class SecretVaultEventHandler : IEventHandler<SetSecretEvent>, IEventHandler<RemoveSecretEvent>
    {
        private readonly ReadStoreContext _context;

        public SecretVaultEventHandler(ReadStoreContext context)
        {
            _context = context;
        }

        public async Task Handle(SetSecretEvent payload)
        {
            var entity = _context.SecretsVault.FirstOrDefault(x => x.Key == payload.Key) ?? new SecretsVaultEntity();
            entity.Key = payload.Key;
            entity.Value = payload.Value;
            _context.SecretsVault.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Handle(RemoveSecretEvent payload)
        {
            var toRemove = _context.SecretsVault.Where(x => x.Key == payload.Key);
            _context.SecretsVault.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }
    }
}
