using System;
using System.Collections.Generic;
using System.Text;

namespace Hogwarts.Model
{
    public class Wizard
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? House { get; set; }
        public string? Patronus { get; set; }
        public int Class { get; set; }
    }
}
