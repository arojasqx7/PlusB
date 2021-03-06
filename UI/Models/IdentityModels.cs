﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Domain.Entities;

namespace UI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        //CustomerID & ConsultantID Extended properties NEW COLUMNS 
        public string CustomerID { get; set; }
        public string ConsultantID { get; set; }
        public int getNotification { get; set; }

        public ApplicationUser()
        : base()
        {
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            /// <summary>
            /// This method evaluates the role of the user who wants to login and 
            /// as per the role evaluate & assign the respective claim.
            /// </summary>

            var role = userIdentity.Claims.ElementAt(4).Value;
            switch (role)
            {
                case "Consultant":
                    userIdentity.AddClaim(new Claim("ConsultantID", this.ConsultantID.ToString()));
                    break;
                case "Customer":
                    userIdentity.AddClaim(new Claim("CustomerID", this.CustomerID.ToString()));
                    break;
                default:
                    return userIdentity;
            }
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("PlusBContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class PreviousPassword
    { 
        public PreviousPassword()
        {
            CreateDate = DateTimeOffset.Now;
        }

        [Key, Column(Order = 0)]
        public string PasswordHash { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}