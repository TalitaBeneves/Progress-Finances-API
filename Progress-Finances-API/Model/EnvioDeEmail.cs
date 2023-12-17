namespace Progress_Finances_API.Model
{
    public class EnvioDeEmail
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Username { get; set; }
        public string? Token{ get; set; }
    }
}
