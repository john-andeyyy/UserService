namespace UserService.Models.Response
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public List<ApiData> Data { get; set; }
    }

    public class ApiData
    {
        public string CredentialId { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderTypeId { get; set; }
        public string ProviderTypeName { get; set; }
        public string ProgramId { get; set; }
        public string Endpoint { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public string Action { get; set; }
        public string Service { get; set; }
    }

}
