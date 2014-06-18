using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Model.Models;

namespace Model.Security
{
    public class List9User:IdentityUser
    {

        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<Project> Projects { get; set; }

        public List9User(string id,string username, string name, string email, string phonenumber)
        {
            this.Id = id;
            this.UserName = username;
            this.Name = name;
            this.Type = null;
            this.Email = email;
            this.PhoneNumber = phonenumber;
       }
        

        public List9User() {
            Projects = new List<Project>();
        }
    }

}
