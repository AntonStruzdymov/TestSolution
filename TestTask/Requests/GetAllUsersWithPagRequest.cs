using System.ComponentModel.DataAnnotations;

namespace TestTask.Requests
{
    public class GetAllUsersWithPagRequest
    {
        [Required]
        public int PageSize { get; set; }
        [Required]
        public int PageNumber { get; set; }        
        public string OrderBy { get; set; }
        public string FilterBy { get; set; }
        public string FilterByValue { get; set; }
    }
}
