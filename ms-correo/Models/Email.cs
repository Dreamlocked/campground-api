namespace ms_correo.Models
{
    public class Email
    {
        public List<String> Recipients { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
