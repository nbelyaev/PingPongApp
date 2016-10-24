using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPongApp2 {
    public class Participant {
        public string Name { get;set;}

        public Participant() {
            Name = "";
        }
        public Participant(string name) {
            Name = name;
        }

        public override bool Equals(object obj) {
            return ((Participant)obj).Name == Name;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

    
    }
}
