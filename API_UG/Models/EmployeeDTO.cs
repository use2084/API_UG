namespace API_UG.Models
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }

        public string EmployeeFirstName { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public string? EmployeePatronymic { get; set; }

        public DateOnly EmployeeBirthday { get; set; }

        public string EmployeePassport { get; set; } = null!;

        public string EmployeePhone { get; set; } = null!;

        public string? EmployeePost { get; set; } 

        public string EmployeeLogin { get; set; } = null!;

        public string EmployeePassword { get; set; } = null!;

        public int EmployeeStatus { get; set; }
    }

}
