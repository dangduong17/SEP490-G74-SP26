namespace vn.edu.fpt.dto
{
    public class AdminUserListViewModel
    {
        public List<AdminUserListItemViewModel> Users { get; set; } = new();

        public string? Keyword { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }

        public int TotalPages => TotalItems == 0 ? 1 : (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
