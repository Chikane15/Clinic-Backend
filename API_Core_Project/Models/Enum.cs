namespace API_Core_Project.Models
{

    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum BloodType
    {
        APositive,
        ANegative,
        BPositive,
        BNegative,
        ABPositive,
        ABNegative,
        OPositive,
        ONegative,
    }

    public enum ReportType
    {
        Consulting,
        XRay,
        BloodReport,
        UrineReport
    }

    public enum Insurance
    {
        Individual,
        SeniorCitizen,
        None
    }

    public enum DoctorSpecialty
    {
        GeneralMedicine,
        Pediatrics,
        Cardiology,
        Orthopedics,
        Gynecology,
        Dermatology,
        Ophthalmology,
        Urology,
        ENT,
        Radiology,
        Dental
    }
}
