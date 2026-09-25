namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class TokenResultDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public TokenResultDto(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }
    }


}
