namespace UserService.Models.Request
{
    public class ProviderRequest
    {
        public int Party_Id { get; set; }
        public string Party_API_ENDPOINT { get; set; }
        public string Party_API_NAME { get; set; }
        public string Party_METHOD { get; set; }
        public string Party_CODE { get; set; }


    }
}