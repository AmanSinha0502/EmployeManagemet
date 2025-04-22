namespace DapperMvcDemo.Models
{
    public class AddPersonDto
    {
        public required string Name { get; set; }  // Required name
        public required string Email { get; set; } // Required email
        public string? Address { get; set; } // Optional address
        public int DeptId { get; set; }  // Required Department ID (foreign key to Department table)
    }
}
