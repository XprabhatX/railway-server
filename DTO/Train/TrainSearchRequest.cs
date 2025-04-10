namespace Railway.DTOs
{
    public class TrainSearchRequest
    {
        public string FromStation {get; set;}
        public string ToStation {get; set;}
        public DateTime Date {get; set;}
        public float? MinRating {get; set;}
    }
}