    namespace fournisseurIdentite.src.DTO;
    
    public class VerifyPasswordRequest
    {
        public string? Password { get; set; }
        public string? StoredHash { get; set; }
    }