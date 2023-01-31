namespace UserServiceRequestMessages
{
    public interface RegisterRequest
    {
         string Title { get; }

         string Firstname { get; }

         string Lastname { get; }

         string IdentificationNumber { get; }

         string Email { get; }

         string Password { get; }

    }
}
