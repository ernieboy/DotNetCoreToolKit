using System;
using DotNetCoreToolKit.Library.Models.Persistence;

namespace DotNetCoreToolKit.Library.Tests.Models
{
    public class Customer : AggregateRoot
    {

        public Customer()
        {

        }
        public Customer(string firstName, string lastName, Guid id)
        {
            Firstname = firstName;
            Lastname = lastName;
            Id = id;
        }

        public string Firstname { get; set; }

        public string Lastname { get; set; }    
    }
}
