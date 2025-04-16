namespace casus_ouderenzorg.Models
{
    public class ContactPerson
    {
        public int Id { get; set; }

        // Full name of the contact person.
        public string Name { get; set; } = string.Empty;

        // Email address.
        public string Email { get; set; } = string.Empty;

        // Phone number.
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
