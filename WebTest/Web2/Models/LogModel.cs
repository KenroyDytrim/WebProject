using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2.Models
{
	public class LogMod
	{
		[Required(ErrorMessage = "Не указан логин")]
		public string Login { get; set; }
		[Required(ErrorMessage = "Не указан пароль")]
		public string Password { get; set; }
	}
}
