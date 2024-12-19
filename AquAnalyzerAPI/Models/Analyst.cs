using System;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Models
{
    public class Analyst : User
    {
        public List<Abnormality> IdentifiedAbnormalities { get; set; }
        public Analyst()
        {
        }

        public Analyst(int id, string username) : base(id, username)
        {
        }

        public Analyst(int id, string username, string password, string email, string role) : base(id, username, password, email, role)
        {
        }
    }
}