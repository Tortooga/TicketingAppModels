using System.Data.Common;
using System.Security.Cryptography;

namespace Models.Projects
{
    class Project : ORMModel<Project>
    {
        private DateTime? _endDate;

        //FileDB required Methods/Properties
        public static readonly Table table = new Table("Projects", [
            //input the fields
        ]);
        public override int? Id { get; set; }
        public override string Name { get; set; }
        protected override Table TableI() => table;
        protected override Dictionary<string, object> GetFields() => new Dictionary<string, object>
        {
            ["Id"] = Id
            // TODO: Add the rest of the fields
        };
        

        //Model Properties
        public ProjectType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime StartDate { get; set; } //Should it be read-only?

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (!(value == null) && !(Status == ProjectStatus.Completed || Status == ProjectStatus.Cancelled))
                {
                    Table.log($"Error: Attempted to set an end date to the {Name} project with Status: {Status}");
                    throw new Exception("Date Entry Validation Error, Check Logs for Details.");
                }
                if (value < StartDate)
                {
                    Table.log($"Error: Attempted to set end date less than start date for the project: {Name}");
                }
                _endDate = value;
            }
        }

        public Project(string Name, ProjectType Type, string Title, string Description, ProjectStatus Status, DateTime StartDate, DateTime EndDate)
        {
            this.Name = Name;
            this.Type = Type;
            this.Title = Title;
            this.Description = Description;
            this.Status = Status;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
        }
    }

    class ProjectType
    {   //TODO:include icons 
        public string Name { set; get; }
        public bool IsPerpetual { set; get; }
    }

    enum ProjectStatus
    {
        Active,
        Completed,
        OnHold,
        Cancelled,
        NotStarted
    }
}