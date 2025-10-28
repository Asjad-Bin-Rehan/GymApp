namespace BS.Services.AuthService.DTOs
{
    public class RequestSignUp
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Super";
        public string Email { get; set; } = "quality@inspection.com";
        public string Password { get; set; } = "Quality!23";
        public string UserType { get; set; } = "SuperAdmin";
        public string Token { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6InF1YWxpdHlAaW5zcGVjdGlvbi5jb20iLCJVc2VySWQiOiI3Mjk5MDY2My0yZWRjLTRjMTAtYjMzMS1jZDFjNjVlNDc3ZTAiLCJVc2VyVHlwZSI6IlN1cGVyQWRtaW4iLCJSZXNvdXJjZXMiOiIiLCJuYmYiOjE3NDI1ODY1NTcsImV4cCI6NDg5NjE4NjU1NywiaWF0IjoxNzQyNTg2NTU3fQ.3bD3WQ-Ifa80fzLjEWojKeD7U6ZlPFvTxZJAoXntKMo";
        public string RefreshToken { get; set; } = "72990663-2edc-4c10-b331-cd1c65e477e0";
    }
}