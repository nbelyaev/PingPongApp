using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPongApp2 {
    class Game {
        public string player1 { get; set; }
        public string player2 { get; set; }
        public string score1 { get; set; }
        public string score2 { get; set; }
        public string timeMin { get; set; }
        public string timeSec { get; set; }
        public string timeStamp { get; }

        public Game() {
            player1 = "Player1";
            player2 = "Player2";
            score1  = "0";
            score2 = "0";
            timeMin  ="0";
            timeSec = "0";
            timeStamp = DateTime.Now.ToString();
            }
        



        public override string ToString() {
            return player1 + " vs. " + player2 + " <" + score1 + ":" + score2 +
            "> Time: " + timeMin + " min. " + timeSec + " sec.";
        }
        //string log = plr1 + " vs. " + plr2 + " <" + scr1 + ":" + scr2 +
        //    "> Time: " + timeMin + " min. " + timeSec + " sec.";
    }
}
