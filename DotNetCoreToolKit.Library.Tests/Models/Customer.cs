using System;
using DotNetCoreToolKit.Library.Models.Persistence;

namespace DotNetCoreToolKit.Library.Tests.Models
{
    public class Customer : BaseObjectWithState
    {

        public Customer()
        {

        }
        public Customer(string firstName, string lastName, Guid guid)
        {
            Firstname = firstName;
            Lastname = lastName;
            Guid = guid;
        }

        public string Firstname { get; set; }

        public string Lastname { get; set; }    
    }
}
