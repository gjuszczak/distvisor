using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads;

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
        private readonly ReadStore _context;

        public SecretVaultEventHandler(ReadStore context)
        {
            _context = context;
        }

        public void Handle(SetSecretEvent payload)
        {
            var entity = _context.SecretsVault.FindOne(x => x.Key == payload.Key) ?? new SecretsVaultEntity();
            entity.Key = payload.Key;
            entity.Value = payload.Value;
            _context.SecretsVault.Upsert(entity);
        }

        public void Handle(RemoveSecretEvent payload)
        {
            _context.SecretsVault.DeleteMany(x => x.Key == payload.Key);
        }
    }
}
