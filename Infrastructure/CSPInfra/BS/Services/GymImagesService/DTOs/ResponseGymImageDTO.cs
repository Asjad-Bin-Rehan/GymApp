namespace BS.Services.GymImagesService.DTOs
{
    public class ResponseGymImageDTO
    {
        public int image_id { get; set; }
        public int gym_id { get; set; }
        public string image_url { get; set; }
        public DateTime added_at { get; set; }
    }

}
