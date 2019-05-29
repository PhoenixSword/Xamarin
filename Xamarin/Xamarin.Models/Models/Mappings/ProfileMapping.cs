namespace Xamarin.Models.Models.Mappings
{
    public static class ProfileMapping
    {
        public static Profile Map(this Profile profile)
        {
            return new Profile
            {
                Id = profile.Id,
                Image = profile.Image,
                Name = profile.Name,
            };
        }
    }
}
