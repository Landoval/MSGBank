using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGBank.Model
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} ID: {Id}";
        }
    }
}
