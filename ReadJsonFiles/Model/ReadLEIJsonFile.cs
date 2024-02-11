namespace ReadJsonFiles.Model
{
    public class ReadLEIJsonFile
    {
        public List<LegalEntity> records { get; set; }
    }
    public class LegalEntity
    {
        public string LEI { get; set; }
        public EntityDetails Entity { get; set; }
        public RegistrationDetails Registration { get; set; }
        public ExtensionDetails Extension { get; set; }
    }

    public class EntityDetails
    {
        public string LegalName { get; set; }
        public Address LegalAddress { get; set; }
        public Address HeadquartersAddress { get; set; }
        public RegistrationAuthority RegistrationAuthority { get; set; }
        public string LegalJurisdiction { get; set; }
        public string EntityCategory { get; set; }
        public LegalForm LegalForm { get; set; }
        public string EntityStatus { get; set; }
        public string EntityCreationDate { get; set; }
    }

    public class Address
    {
        public string LegalAddressxmllang { get; set; }
        public string LegalNamexmllang { get; set; }
        public string FirstAddressLine { get; set; }
        public string AdditionalAddressLine { get; set; }
        public string FirstAddressNumber { get; set; }
        public string AddressNumberWithinBuilding { get; set; }
        public string MailRouting { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }

    public class RegistrationAuthority
    {
        public string RegistrationAuthorityID { get; set; }
        public string RegistrationAuthorityEntityID { get; set; }
    }

    public class LegalForm
    {
        public string EntityLegalFormCode { get; set; }
        public string OtherLegalForm { get; set; }
    }

    public class RegistrationDetails
    {
        public string RegistrationAuthorityID { get; set; }
        public string OtherRegistrationAuthorityID { get; set; }
        public string RegistrationAuthorityEntityID { get; set; }
        public string InitialRegistrationDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string RegistrationStatus { get; set; }
        public string NextRenewalDate { get; set; }
        public string ManagingLOU { get; set; }
        public string ValidationSources { get; set; }
        public ValidationAuthority ValidationAuthority { get; set; }
    }

    public class ValidationAuthority
    {
        public string ValidationAuthorityID { get; set; }
        public string ValidationAuthorityEntityID { get; set; }
        public string OtherValidationAuthorityID { get; set; }
    }
    public class OtherEntityNames
    {
        public string OtherEntityNamexmllang { get; set; }
        public string type { get; set; }
        public string OtherEntityName { get; set; }
    }
    public class TransliteratedOtherEntityNames
    {
        public string TranOtherEntityNamexmllang { get; set; }
        public string TranOtherEntitytype { get; set; }
        public string TranOtherEntityName { get; set; }
    }
    public class OtherAddresses
    {
        public string xmllang { get; set; }
        public string type { get; set; }
        public string FirstAddressLine { get; set; }
        public string AddressNumber { get; set; }
        public string AddressNumberWithinBuilding { get; set; }
        public string MailRouting { get; set; }
        public OtherAdditionalAddresses OtherAdditionalAddresses { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }

    public class TransliteratedOtherAddresses
    {
        public string xmllang { get; set; }
        public string type { get; set; }
        public string FirstAddressLine { get; set; }
        public string AddressNumber { get; set; }
        public string AddressNumberWithinBuilding { get; set; }
        public string MailRouting { get; set; }
        public OtherAdditionalAddresses OtherAdditionalAddresses { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }

    public class OtherAdditionalAddresses
    {
        public string AdditionalAddressLine { get; set; }
    }
    public class ExtensionDetails
    {

    }
}
