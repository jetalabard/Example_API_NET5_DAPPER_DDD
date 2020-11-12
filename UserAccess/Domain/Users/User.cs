using Common.Domain.BuildingBlocks;
using Common.Domain.Emails;
using UserAccess.Domain.Roles;

namespace UserAccess.Domain.Users
{
    public class User: AggregateRoot<UserId>
    {
        public FirstName FirstName { get; private set; }

        public LastName LastName { get; private set; }

        public Email Email { get; private set; }

        public Role Role { get; set; }

        public IsActive IsActive { get; private set; }

        public User()
        {
            IsActive = new IsActive(true);
        }

        private User(UserId id, FirstName firstName, LastName lastName, Email email, Role role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            IsActive = new IsActive(true);
        }

        public static User Create(UserId id, FirstName firstName, LastName lastName, Email email, Role role) => new User(id, firstName, lastName, email, role);

        public void UpdateFirstName(string firstName)
        {
            FirstName = new FirstName(firstName);
        }

        public void UpdateIsActive(bool active)
        {
            IsActive = new IsActive(active);
        }

        public void UpdateLastName(string lastName)
        {
            LastName = new LastName(lastName);
        }

        public void UpdateEmail(string email)
        {
            Email = new Email(email);
        }

        public void UpdateRole(Role role)
        {
            Role = role;
        }
    }
}
