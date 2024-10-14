using System.Text.Json.Serialization;

namespace WebAppCrud.Models
{
	public class AuthModel
	{
        public Credentials Credentials { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public AuthType AuthType { get; set; }
    }
}
