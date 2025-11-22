namespace BS.Services.GymImagesService.DTOs
{
    public class AddGymImageDTO
    {
        /// <summary>
        /// Gym ID (must exist in PartnerGyms)
        /// </summary>
        public int gym_id { get; set; }

        /// <summary>
        /// Image URL (Cloudinary or any public URL)
        /// </summary>
        public string image_url { get; set; } = null!;
    }
}
