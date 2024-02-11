namespace ReadJsonFiles.Model
{
    public class ReadJsonFile
    {
        public string RelationshipStartNodeNodeID { get; set; }
        public string RelationshipStartNodeNodeIDType { get; set; }
        public string RelationshipEndNodeNodeID { get; set; }
        public string RelationshipEndNodeNodeIDType { get; set; }
        public string RelationshipType { get; set; }
        public string RelationshipStatus { get; set; }
        public string RelationshipPeriod1StartDate { get; set; }
        public string RelationshipPeriod1EndDate { get; set; }
        public string RelationshipPeriod1PeriodType { get; set; }
        public string RelationshipPeriod2StartDate { get; set; }
        public string RelationshipPeriod2EndDate { get; set; }
        public string RelationshipPeriod2PeriodType { get; set; }
        public string RelationshipPeriod3StartDate { get; set; }
        public string RelationshipPeriod3EndDate { get; set; }
        public string RelationshipPeriod3PeriodType { get; set; }
        public string RelationshipPeriod4StartDate { get; set; }
        public string RelationshipPeriod4EndDate { get; set; }
        public string RelationshipPeriod4PeriodType { get; set; }
        public string RelationshipPeriod5StartDate { get; set; }
        public string RelationshipPeriod5EndDate { get; set; }
        public string RelationshipPeriod5PeriodType { get; set; }
        public string RelationshipQualifiers1QualifierDimension { get; set; }
        public string RelationshipQualifiers1QualifierCategory { get; set; }
        public string RelationshipQualifiers2QualifierDimension { get; set; }
        public string RelationshipQualifiers2QualifierCategory { get; set; }
        public string RelationshipQualifiers3QualifierDimension { get; set; }
        public string RelationshipQualifiers3QualifierCategory { get; set; }
        public string RelationshipQualifiers4QualifierDimension { get; set; }
        public string RelationshipQualifiers4QualifierCategory { get; set; }
        public string RelationshipQualifiers5QualifierDimension { get; set; }
        public string RelationshipQualifiers5QualifierCategory { get; set; }
        public string RelationshipQuantifiers1MeasurementMethod { get; set; }
        public string RelationshipQuantifiers1QuantifierAmount { get; set; }
        public string RelationshipQuantifiers1QuantifierUnits { get; set; }
        public string RelationshipQuantifiers2MeasurementMethod { get; set; }
        public string RelationshipQuantifiers2QuantifierAmount { get; set; }
        public string RelationshipQuantifiers2QuantifierUnits { get; set; }
        public string RelationshipQuantifiers3MeasurementMethod { get; set; }
        public string RelationshipQuantifiers3QuantifierAmount { get; set; }
        public string RelationshipQuantifiers3QuantifierUnits { get; set; }
        public string RelationshipQuantifiers4MeasurementMethod { get; set; }
        public string RelationshipQuantifiers4QuantifierAmount { get; set; }
        public string RelationshipQuantifiers4QuantifierUnits { get; set; }
        public string RelationshipQuantifiers5MeasurementMethod { get; set; }
        public string RelationshipQuantifiers5QuantifierAmount { get; set; }
        public string RelationshipQuantifiers5QuantifierUnits { get; set; }
        public string RegistrationInitialRegistrationDate { get; set; }
        public string RegistrationLastUpdateDate { get; set; }
        public string RegistrationStatus { get; set; }
        public string RegistrationNextRenewalDate { get; set; }
        public string RegistrationManagingLOU { get; set; }
        public string RegistrationValidationSources { get; set; }
        public string RegistrationValidationDocuments { get; set; }
        public string RegistrationValidationReference { get; set; }
        public string DeletedAt { get; set; }
        public List<RelationshipPeriods> relationshipPeriods { get; set; }
        public List<RelationshipQualifiers> relationshipQualifiers { get; set; }
        public List<RelationshipQuantifiers> relationshipQuantifiers { get; set; }
    }
    public class RelationshipPeriods
    {
        public string RelationshipPeriodPeriodType { get; set; }
        public string RelationshipPeriodStartDate { get; set; }
        public string RelationshipPeriodEndDate { get; set; }
    }
    public class RelationshipQualifiers
    {
        public string RelationshipQualifiersQualifierDimension { get; set; }
        public string RelationshipQualifiersQualifierCategory { get; set; }
    }
    public class RelationshipQuantifiers
    {
        public string RelationshipQuantifiersMeasurementMethod { get; set; }
        public string RelationshipQuantifiersQuantifierAmount { get; set; }
        public string RelationshipQuantifiersQuantifierUnits { get; set; }
    }

}
