using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPongApp2 {
    class Participant {
        public string Name { get;set;}

        public Participant() {
            Name = "";
        }
        public Participant(string name) {
            Name = name;
        }
    }
}
