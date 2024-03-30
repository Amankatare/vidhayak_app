namespace VidhayakApp.Web.ViewModels
{

    public class DashboardViewModel
    {
        public int PendingComplaintsCount { get; set; }
        public int PendingDemandsCount { get; set; }
        public int PendingSuggestionsCount { get; set; }
        public int TotalComplaintsCount { get; set; }
        public int TotalDemandsCount { get; set; }
        public int TotalSuggestionsCount { get; set; }
        public int UsersInWardCount { get; set; }
        public int UsersTotalCount { get; set; }

        public string DepartmentName { get; set; }
        public int TotalItemCount { get; set; }

        public int OtherCount { get; set; }
        public int PendingCount { get; set; }
        public int InProgressCount { get; set; }
        public int RejectedCount { get; set; }
        public int CompletedCount { get; set; }
    }
}
