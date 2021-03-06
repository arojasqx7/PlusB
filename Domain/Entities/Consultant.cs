﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Consultant
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string IdNumber { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Pais { get; set; }

        public string Address { get; set; }

        public int PhoneNumber { get; set; }

        public string JobTitle { get; set; }

        public string Education { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + "  " + LastName;
            }
        }
    }
}
