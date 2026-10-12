using System.ComponentModel.DataAnnotations;

namespace InterviewScheduler.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Password { get; set; }
        public string URl { get; set; }
        public string ErrorMessage { get; set; }
        public bool RememberMe { get; set; }
    }
}