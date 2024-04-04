namespace VidhayakApp.Web.ViewModels
{
    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ComplaintsCount { get; set; }
        public int SuggestionsCount { get; set; }
        public int DemandsCount { get; set; }
        public int OtherCount { get; set; }
        public int PendingCount { get; set; }
        public int InProgressCount { get; set; }
        public int RejectedCount { get; set; }
        public int CompletedCount { get; set; }

        public List<DepartmentViewModel> Departments { get; set; }
    }
}
