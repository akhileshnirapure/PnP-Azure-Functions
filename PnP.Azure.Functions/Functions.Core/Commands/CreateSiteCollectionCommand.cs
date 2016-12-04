namespace Functions.Core.Commands
{
    public class CreateSiteCollectionCommand : ICommand
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Template { get; set; }
        public string PrimaryOwnerEmail { get; set; }
        public string SecondaryOwnerEmail { get; set; }
        public int StorageMaximumLevel { get; set; }
        public int UserCodeMaximumLevel { get; set; }
        public string TenantName { get; set; }
        public string AdditionalRequirement { get; set; }
    }
}
