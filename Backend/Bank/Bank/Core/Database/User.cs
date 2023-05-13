﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Database
{
    [Table("User")]
    public class User
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public User(string name, string password, string email)
        {
            Name = name;
            Password = password;
            Email = email;
        }

    }
}
