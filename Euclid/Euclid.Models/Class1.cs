namespace Euclid.Dtos
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public string? Errors { get; set; } 
    }
}