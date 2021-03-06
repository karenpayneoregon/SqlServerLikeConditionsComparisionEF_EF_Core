﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCoreLikeLibrary
{
    public class CustomerEntity
    {
        public int CustomerIdentifier { get; set; }

        public string CompanyName { get; set; }
        public int? ContactIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string ContactName => $"{FirstName} {LastName}";
        public int ContactTypeIdentifier { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? CountryIdentifier { get; set; }
        public string CountyName { get; set; }
    }
}
