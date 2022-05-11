namespace PersonManagement.Api.Models
{
	public class RegisterModel
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

        public RegisterModel()
        {
        }

        public RegisterModel(string userName, string email, string password)
        {
			UserName = userName;
			Email = email;
			Password = password;
        }
	}
}
