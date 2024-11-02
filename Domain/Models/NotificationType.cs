namespace Domain.Models
{
	public enum NotificationType
	{
		None = 0,
		Get = 1,
		Create,
		Update,
		Delete,
		Validate,
		Information,
		Warning,
		Error,
		Register,
		RegistrationError,
		Auth,
		AuthError,
		DeleteUser
	}
}
