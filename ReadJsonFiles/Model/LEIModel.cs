using System.Xml.Serialization;

namespace ReadJsonFiles.Model
{
    public class LEIModel
    {
        public string LEI { get; set; }
        public string EntityLegalName { get; set; }
        public string EntityLegalAddressFirstAddressLine { get; set; }
        public string EntityLegalAdditionalAddressLine1 { get; set; }
        public string EntityLegalAddressCity { get; set; }
        public string EntityLegalAddressRegion { get; set; }
        public string EntityLegalAddressCountry { get; set; }
        public string EntityLegalAddressPostalCode { get; set; }
        public string EntityHeadquartersAddressFirstAddressLine { get; set; }
        public string EntityHeadquartersAddressAdditionalAddressLine1 { get; set; }
        public string EntityHeadquartersAddressCity { get; set; }
        public string EntityHeadquartersAddressRegion { get; set; }
        public string EntityHeadquartersAddressCountry { get; set; }
        public string EntityHeadquartersAddressPostalCode { get; set; }
        public string EntityRegistrationAuthorityRegistrationAuthorityID { get; set; }
        public string EntityRegistrationAuthorityRegistrationAuthorityEntityID { get; set; }
        public string EntityLegalJurisdiction { get; set; }
        public string EntityEntityCategory { get; set; }
        public string EntityLegalFormEntityLegalFormCode { get; set; }
        public string EntityEntityStatus { get; set; }
        public string EntityCreationDate { get; set; }
        public string RegistrationInitialRegistrationDate { get; set; }
        public string RegistrationLastUpdateDate { get; set; }
        public string RegistrationRegistrationStatus { get; set; }
        public string RegistrationNextRenewalDate { get; set; }
        public string RegistrationManagingLOU { get; set; }
        public string RegistrationValidationSources { get; set; }
        public string RegistrationValidationAuthorityValidationAuthorityID { get; set; }
        public string RegistrationValidationAuthorityValidationAuthorityEntityID { get; set; }
    }
}
