namespace Support.Application.Rfas.Outputs
{
    public record RfaRequest(string PhoneNumber, string Profession, string LastName, string FirstName, string Email);

    public record RfaOutput(string PhoneNumber, string Profession, RfaUserOutput User);

    public record RfaUserOutput(string LastName, string FirstName, string Email);
}
