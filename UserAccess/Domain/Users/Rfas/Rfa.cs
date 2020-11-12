
using Common.Domain.BuildingBlocks;
using Common.Domain.Emails;
using System;

namespace UserAccess.Domain.Users.Rfas
{
    public class Rfa : Entity<RfaId>
    {
        public UserId UserId { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public Profession Profession { get; private set; }

        private Rfa(RfaId id, UserId userId, Email email, PhoneNumber phoneNumber, Profession profession)
        {
            Id = id;
            UserId = userId;
            Email = email;
            PhoneNumber = phoneNumber;
            Profession = profession;
        }

        public static Rfa Create(UserId userId, Email email, PhoneNumber phoneNumber, Profession profession)
        {
            return new Rfa(new RfaId(Guid.NewGuid()), userId, email, phoneNumber, profession);
        }

        public static Rfa Update(Rfa toUpdate, Email email, PhoneNumber phoneNumber, Profession profession)
        {
            toUpdate.Email = email;
            toUpdate.PhoneNumber = phoneNumber;
            toUpdate.Profession = profession;
            return toUpdate;
        }

    }
}
