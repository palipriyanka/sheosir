using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewCore3xMVC.Models.Views
{
    public class PersonAddressView
    {

        [Key]
        public int AutoId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public bool Active { get; set; }

        public int AddressId { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string Pin { get; set; }

    }
}
