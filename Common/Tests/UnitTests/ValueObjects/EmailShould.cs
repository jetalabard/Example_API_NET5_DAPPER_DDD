using Common.Domain.Emails;
using FluentAssertions;
using System;
using Xunit;

namespace Common.Tests.UnitTests
{
    public class EmailShould
    {
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        [InlineData("email.fr")]
        [InlineData("email@fr")]
        [InlineData("@domain.fr")]
        [InlineData("email@domain")]
        [InlineData("email@-domain.com")]
        [InlineData("email@+domain.com")]
        public void ThrowWhenEmailIsInvalid(string email)
        {
            Action action = () => _ = new Email(email);
            action.Should().Throw<InvalidMailFormatException>();
        }

        [Theory]
        [InlineData("email@domain.fr")]
        [InlineData("eMail@Domain.Fr")]
        [InlineData("e.ma-il@do.main.fr")]
        [InlineData("e.ma-il@my.do.main.fr")]
        [InlineData("e.ma-il@my-do.main.fr")]
        [InlineData("TEST@TEST.COM")]
        [InlineData("test@DO.MAIN.COM")]
        [InlineData("!#$%&\'*+-/=?^_`.{|}~@example.com")]
        [InlineData("#!$%&\'*+-/=?^_`{}|~@example.org")]
        [InlineData("email@1domain.com")]
        public void NotThrowWhenEmailIsValid(string email)
        {
            Action action = () => _ = new Email(email);
            action.Should().NotThrow();
        }

    }
}
