using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPongApp2 {
    public class Game {
        public string player1 { get; set; }
        public string player2 { get; set; }
        public string score1 { get; set; }
        public string score2 { get; set; }
        public string timeMin { get; set; }
        public string timeSec { get; set; }
        public string timeStamp { get; }
        public int victoryScoreIndex { get; set; }
        public enum colorOptions {p1Serves,p2Serves,p1Won, p2Won }
        public colorOptions colorSetup;
        public bool singleServe;
        public Stopwatch gameStopwatch { get; set; }
        public Stack<int> history { get; set; }

        public Game() {
            player1 = "Player1";
            player2 = "Player2";
            score1  = "0";
            score2 = "0";
            timeMin  ="0";
            timeSec = "0";
            timeStamp = DateTime.Now.ToString();
            history = new Stack<int>();
        }
        

        public int GetWinner() {
            int response;
            if(int.Parse( score1)> int.Parse(score2)) {
                response = 1;
            }
            else if (int.Parse(score1) < int.Parse(score2)) {
                response = 2;
            }
            else {
                response = 0;
            }
            return response;
        }

        public override string ToString() {
            return player1 + " vs. " + player2 + " <" + score1 + ":" + score2 +
            "> Approx. Time: " + timeMin + " min. " + timeSec + " sec.";
        }
    }
}
