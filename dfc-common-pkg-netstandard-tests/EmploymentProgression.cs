using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace NCS.DSS.CosmosDocumentClient.Tests
{
    public class EmploymentProgression
    {
        private const string PostcodeRegEx = @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})";

        [Display(Description = "Unique identifier for a Employment Progression record")]
        [JsonProperty(PropertyName = "id")]
        public Guid? EmploymentProgressionId { get; set; }

        [Display(Description = "Unique identifier of a customer")]
        [Required]
        public Guid? CustomerId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Description = "Date and time employment progression was created.")]
        [Required]
        public DateTime? DateProgressionRecorded { get; set; }  

        [Display(Description = "Economic Shock Code")]
        [StringLength(50)]
        public string EconomicShockCode { get; set; }

        [Display(Description = "Name of the employer")]
        [StringLength(200)]
        public string EmployerName { get; set; }

        [Display(Description = "Address of employer")]
        [StringLength(500)]
        public string EmployerAddress { get; set; }

        [StringLength(10)]
        [RegularExpression(PostcodeRegEx, ErrorMessage = "Please enter a valid postcode")]
        [Display(Description = "Employer postcode")]
        public string EmployerPostcode { get; set; }

        [RegularExpression(@"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$")]
        [Display(Description = "Geocoded address information")]
        public decimal? Latitude { get; set; }

        [RegularExpression(@"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$")]
        [Display(Description = "Geocoded address information")]
        public decimal? Longitude { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Description = "Date and time of employment.")]
        public DateTime? DateOfEmployment { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Description = "Date and time of last employment.")]
        public DateTime? DateOfLastEmployment { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Description = "Date and time last modified.")]
        public DateTime? LastModifiedDate { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Display(Description = "Identifier of the touchpoint who made the last change to the record")]
        public string LastModifiedTouchpointId { get; set; }

        [StringLength(10)]
        public string CreatedBy { get; set; }
    }
}
