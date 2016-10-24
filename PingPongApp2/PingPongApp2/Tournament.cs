using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPongApp2 {
    public class Tournament {
        public Stack<Participant> players { get; set; }
        public Stack<Participant> nextRoundPlayers { get; set; }
        public int roundNum = 1;


        public Tournament(List<Participant> list) {
            list.Shuffle();
            players = new Stack<Participant>();
            nextRoundPlayers = new Stack<Participant>();
            foreach (Participant part in list) {
                players.Push(part);

            }
            
        }

        public bool RemoveParticipant(Participant participant) {
            List<Participant> list = null;
            bool result = false;
            if (players.Contains(participant)) {
                list = players.ToList();
                list.Remove(participant);
                ShufflePlayers();
                ListToStack(list, players);
                result = true;
            }
            else if (nextRoundPlayers.Contains<Participant>(participant)) {
                list = nextRoundPlayers.ToList();
                list.Remove(participant);
                ListToStack(list, nextRoundPlayers);
                result = true;
            }
            return result;
        }

        private void ListToStack<T>(List<T> list, Stack<T> stack) {
            stack.Clear();
            foreach(T item in list) {
                stack.Push(item);
            }
        }


        public void ShufflePlayers() {
            if(players.Count > 0) {
                List<Participant> list = players.ToList();
                list.Shuffle();
                players = new Stack<Participant>();
                //nextRoundPlayers = new Stack<Participant>();
                foreach (Participant part in list) {
                    players.Push(part);

                }

            }
            
        }



        public void AddParticipant(Participant part) {
            if(players.Count > 0) {
                players.Push(part);
                ShufflePlayers();
            }
            else {
                nextRoundPlayers.Push(part);
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
            ShufflePlayers();

            roundNum++;


            NewRoundStartedEventArgs args = new NewRoundStartedEventArgs();
            args.roundNum = roundNum;
            OnNewRoundStarted(args);
        }

        public void AdvancePlayerToNextRound(Participant part) {
            nextRoundPlayers.Push(part);
        }


        protected virtual void OnNewRoundStarted(NewRoundStartedEventArgs e) {
            EventHandler<NewRoundStartedEventArgs> handler = NewRoundStarted;
            if (handler != null) {
                handler(this, e);
            }
        }

        public event EventHandler<NewRoundStartedEventArgs> NewRoundStarted;



    }

    public class NewRoundStartedEventArgs : EventArgs {
        public int roundNum { get; set; }
    }

    static class shuffler {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list) {
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
