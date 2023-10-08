using System.ComponentModel.DataAnnotations;

namespace TestTask.Requests
{
    public class AddUserRequest
    {
        [Required(ErrorMessage = "Не указано имя пользователя")]
        public string UserName { get; set; }
        [Required]
        [Range(1,100, ErrorMessage = "Возраст должен быть в диапазоне {1}-{2}")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Не указано Email адрес")]
        public string Email { get; set; }
    }
}
