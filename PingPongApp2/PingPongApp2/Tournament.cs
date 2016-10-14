using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPongApp2 {
    public class Tournament {
        public Stack<Participant> players { get; set; }
        public Stack<Participant> nextRoundPlayers { get; set; }


        public Tournament(List<Participant> list) {
            //list.
            list.Shuffle();
            foreach (Participant part in list) {
                players.Push(part);

            }


            
        }

        public bool VictorDecided() {
            return (players.Count + nextRoundPlayers.Count) == 1;
        }

        public Game NextGame() {
            Game newgame = new Game();

            if(players.Count == 1) {
                AdvancePlayerToNextRound(players.Pop());
                NextRound();
            }
            else if(players.Count == 0) {
                NextRound();
            }


            newgame.player1 = players.Pop().Name;

            newgame.player2 = players.Pop().Name;

            return newgame;
        }

        public void NextRound() {
            players = nextRoundPlayers;
            nextRoundPlayers = new Stack<Participant>();
        }

        public void AdvancePlayerToNextRound(Participant part) {
            nextRoundPlayers.Push(part);
        }



        


    }

    static class shuffler {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list) {
            //List<T> list =  l.ToList();
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    


}
