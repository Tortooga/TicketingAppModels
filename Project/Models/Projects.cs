using System.Data.Common;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Utilities;
using System.Globalization;

namespace Models.Projects
{
    class Project : ORMModel<Project>
    {
        private DateTime? _endDate;

        //FileDB required Methods/Properties
        public static readonly Table table = new Table("Projects", [
            ["ID", "Int32"],
            ["Name", "String"],
            ["TypeName", "String"],
            ["TypeIsPerPetual", "Boolean"],
            ["Title", "String"],
            ["Description", "String"],
            ["Status", "String"],
            ["StartDate", "String"],
            ["EndDate", "String"]
            //input the fields
        ]);
        public override int? Id { get; set; }
        public override string Name { get; set; }
        protected override Table TableI() => table;
        protected override Dictionary<string, object> GetFields() => new Dictionary<string, object>
        {
            ["Id"] = Id,
            ["Name"] = Name,
            ["TypeName"] = Type.Name,
            ["TypeIsPerpetual"] = Type.IsPerpetual,
            ["Title"] = Title,
            ["Description"] = Description,
            ["Status"] = Status.ToString(),
            //The Following DateTime objects are converted to be stored as strings with a certain format
            ["StartDate"] = StartDate.ToString(Constants.DateTimeFormat),
            ["EndDate"] = EndDate.HasValue ? EndDate.Value.ToString(Constants.DateTimeFormat) : "_", //checking if it has a value before attempting to parse
            // TODO: Add the rest of the fields
            //Remember to convert ProjectStatus into a pair of a string and a bool
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

        //Constructor only with native types for ORM Layer Compatability
        protected Project(string Name, string TypeName, bool TypeIsPerpetual, string Title, string Description, string Status, string StartDate, string EndDate)
        {
            this.Name = Name;
            this.Title = Title;
            this.Description = Description;
            this.StartDate = DateTime.ParseExact(StartDate, Constants.DateTimeFormat, CultureInfo.InvariantCulture);

            if (EndDate == "_")
            {
                this.EndDate = null;
            }
            else
            {
            this.EndDate = DateTime.ParseExact(EndDate, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
            }

            this.Status = (ProjectStatus)Enum.Parse(typeof(ProjectStatus), Status);
            this.Type = new ProjectType(TypeName, TypeIsPerpetual);
        }
        public Project(string Name, ProjectType Type, string Title, string Description, ProjectStatus Status, DateTime StartDate, DateTime? EndDate = null)
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

        public ProjectType(string Name, bool IsPerpetual)
        {
            this.Name = Name;
            this.IsPerpetual = IsPerpetual;
        }
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