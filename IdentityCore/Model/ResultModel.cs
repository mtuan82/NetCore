namespace IdentityCore.Model
{
    public class ResultModel
    {
        public string Token { get; set; }  = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Error { get; set; }  = string.Empty;

        public bool IsSuccessful { get; set; } = false;
    }
}
