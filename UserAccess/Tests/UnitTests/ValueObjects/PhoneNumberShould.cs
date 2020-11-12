using FluentAssertions;
using System;
using UserAccess.Domain.Exceptions;
using UserAccess.Domain.Users.Rfas;
using Xunit;

namespace UserAccess.UnitTests
{
    public class PhoneNumberShould
    {
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        [InlineData("email.fr")]
        [InlineData("012345678")]
        [InlineData("+1234567")]
        public void ThrowWhenEmailIsInvalid(string phoneNumber)
        {
            Action action = () => _ = new PhoneNumber(phoneNumber);
            action.Should().Throw<InvalidPhoneNumberException>();
        }

        [Theory]
        [InlineData("04 01 02 03 04")]
        [InlineData("0123456789")]
        [InlineData("+33 4 01 02 03 04")]
        public void NotThrowWhenEmailIsValid(string phoneNumber)
        {
            Action action = () => _ = new PhoneNumber(phoneNumber);
            action.Should().NotThrow();
        }
    }
}
