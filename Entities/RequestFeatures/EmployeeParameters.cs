namespace CompanyEmployee.Entities.RequestFeatures
{
    public class EmployeeParameters : RequestParameters
    {
        public EmployeeParameters()
        {
            OrderBy = "name";
        }
        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = int.MaxValue;
        public string SearchTerm { get; set; }

        public bool AgeRangeIsValid() => MaxAge > MinAge;
    }
}