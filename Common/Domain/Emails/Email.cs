using Common.Domain.BuildingBlocks;
using System.Text.RegularExpressions;

namespace Common.Domain.Emails
{
    public record Email : SingleValueObject<string>
    {
        private const string EmailPattern =
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public Email(string value) : base(value)
        {
            var valueToTest = value?.ToLower();

            if (string.IsNullOrWhiteSpace(valueToTest) ||
                !Regex.IsMatch(valueToTest, EmailPattern))
            {
                throw new InvalidMailFormatException();
            }
        }
    }
}
