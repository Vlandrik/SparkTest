namespace SparkTest.Services.Models
{
    public class JWTTokenModel
    {
        public string ExpireDate { get; set; }

        public string Type { get; set; }

        public string AccessToken { get; set; }
    }
}
