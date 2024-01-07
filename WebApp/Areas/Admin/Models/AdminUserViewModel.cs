namespace WebApp.Areas.Admin.Models
{
    public class AdminUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FacebookAddress { get; set; } = string.Empty;
        public string InstagramAddress { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public string Roles { get; set; } = string.Empty;
        public string UserImagePath { get; set; }
		public string NewEmail { get; internal set; }
	}
}
