namespace Schools.Services.Models
{
    using System.Diagnostics;
    using FileHelpers;

    [DebuggerDisplay("{Uid}, {Description}")]
    [DelimitedRecord(";")]
    [IgnoreFirst]
    public class School
    {
        public string Netz;
        public string Uid;
        public string SchoolFullName;
        public string Street;
        public string InentoryNumber;
        public string SN;
        [FieldQuoted]
        public string Description;
        public string TypeName;
        public string SapDescription;
        public string DeliveryDate;
        public string ReplacementDate;
        public string Room;
        public string Plug;
        public string ProcurementType;
    }
}
