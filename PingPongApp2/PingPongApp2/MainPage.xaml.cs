﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PingPongApp2 {
    public partial class MainPage : ContentPage {
        private int victoryScore {
            get {
                return int.Parse(PointsPicker.Items[PointsPicker.SelectedIndex]);
            }
            set { }
        }
        private int count { get; set; }//can be replaced with local variables, REMOVE
        public bool victory { get; set; }
        private bool singleServe { get; set; }
        private Stopwatch stopwatch;
        private List<Game> games = new List<Game>();
        private string SAVED_GAMES;
        public Tournament currentTournament;
        public bool tournamentGame;
        public Stack<int> history;



        public MainPage() {
            Title = "Single Match";
            SAVED_GAMES = "match_log.txt";
            tournamentGame = false;

            InitializeComponent();
            PointsPicker.SelectedIndex = 1;
            stopwatch = new Stopwatch();
            player1.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
            player2.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));


            reset.Clicked += ResetBtn;

            reset.IsEnabled = true;

            roundNumber.IsVisible = false;


            Reset();


            LoadGames();


        }

        public Game SaveGame() {
            Game thisGame = new Game();
            thisGame.player1 = player1.Text;
            thisGame.player2 = player2.Text;
            thisGame.score1 = btnPlayer1.Text;
            thisGame.score2 = btnPlayer2.Text;
            thisGame.timeMin = stopwatch.Elapsed.Minutes.ToString();
            thisGame.timeSec = stopwatch.Elapsed.Seconds.ToString();
            thisGame.victoryScoreIndex = PointsPicker.SelectedIndex;
            thisGame.singleServe = singleServe;

            thisGame.history = history;

            stopwatch.Stop();
            thisGame.gameStopwatch = stopwatch;


            if (btnPlayer1.BackgroundColor == Color.Green) {
                thisGame.colorSetup = Game.colorOptions.p1Won;
            }
            else if (btnPlayer2.BackgroundColor == Color.Green) {
                thisGame.colorSetup = Game.colorOptions.p2Won;
            }
            else if (btnPlayer1.BackgroundColor == Color.Blue) {
                thisGame.colorSetup = Game.colorOptions.p1Serves;
            }
            else if (btnPlayer2.BackgroundColor == Color.Blue) {
                thisGame.colorSetup = Game.colorOptions.p2Serves;
            }
            return thisGame;
        }


        //can probably combine this one with MainPage(Tournament tournament, Game game)
        public MainPage(Tournament tournament) {
            //Title = "Tournament Match";
            //SAVED_GAMES = "tournament_log.txt";
            //tournamentGame = true;



            //InitializeComponent();
            //player1.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
            //player2.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));


            currentTournament = tournament;

            currentTournament.NewRoundStarted += NewRoundMessage;



            //reset.Clicked += setUpNextTournamentGame;
            //reset.IsEnabled = false;

            SetUpGame(tournament.NextGame());
            ClearRecords();


            //LoadGames();
            //DisplayGames();


        }
        public MainPage(Tournament tournament, Game game) {
            //Title = "Tournament Match";
            //SAVED_GAMES = "tournament_log.txt";
            //tournamentGame = true;



            //InitializeComponent();
            //player1.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
            //player2.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));


            currentTournament = tournament;

            //currentTournament.NewRoundStarted += NewRoundMessage;


            //reset.Clicked += setUpNextTournamentGame;
            //reset.IsEnabled = false;

            SetUpGame(game);
            //ClearRecords();


            //LoadGames();
            //DisplayGames();

        }
        public void NewRoundMessage(object sender, NewRoundStartedEventArgs e) {

            DisplayAlert("Round", "The round number " + e.roundNum + " have started!", "OK");
            //roundNumber.Text = "Round " + currentTournament.roundNum;
        }

        public void setUpNextTournamentGame(object o, EventArgs e) {

            int gamesPlayed = currentTournament.p1GamesWon + currentTournament.p2GamesWon;
            int gamesToPlay = currentTournament.gamesToWin[currentTournament.gamesToWinIndex];
            //if (!currentTournament.VictorDecided()) {


                if (currentTournament != null && stackGamesToWin.IsVisible) {
                    //GameNumberPicker.IsEnabled = false;
                    currentTournament.newRound = false;
                }


            if(gamesPlayed== gamesToPlay) {
                SetUpGame(currentTournament.NextGame());
            }
            else {
                Reset();
                SetUpGame(SaveGame());
            }


                //SetUpGame(currentTournament.NextGame());
                reset.IsEnabled = false;
            //}
        }

        public void SetUpGame(Game game) {
            Title = "Tournament Match";
            SAVED_GAMES = "tournament_log.txt";
            tournamentGame = true;





            InitializeComponent();
            player1.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
            player2.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));





            stackGamesToWin.IsVisible = true;
            GameNumberPicker.Items.Clear();
            foreach (int n in currentTournament.gamesToWin) {
                GameNumberPicker.Items.Add(n.ToString());
            }
            GameNumberPicker.SelectedIndex = currentTournament.gamesToWinIndex;


            if (!currentTournament.newRound) {
                GameNumberPicker.IsEnabled = false;
            }









            roundNumber.Text = "Round " + currentTournament.roundNum;
            //currentTournament.NewRoundStarted += NewRoundMessage;


            reset.Clicked += setUpNextTournamentGame;
            reset.IsEnabled = false;


            //ClearRecords();


            LoadGames();
            DisplayGames();





            player1.Text = game.player1;
            player1.IsEnabled = false;
            player2.Text = game.player2;
            player2.IsEnabled = false;
            btnPlayer1.Text = game.score1;
            btnPlayer2.Text = game.score2;
            PointsPicker.SelectedIndex = game.victoryScoreIndex;
            singleServe = game.singleServe;


            history = game.history;
            if (history != null && history.Count > 0) {
                btnUndo.IsEnabled = true;
            }


            btnPlayer1.BackgroundColor = Color.Gray;
            btnPlayer1.BackgroundColor = Color.Gray;

            switch (game.colorSetup) {

                case Game.colorOptions.p1Won:
                    btnPlayer1.BackgroundColor = Color.Green;
                    victory = true;
                    reset.IsEnabled = true;
                    break;
                case Game.colorOptions.p2Won:
                    btnPlayer2.BackgroundColor = Color.Green;
                    victory = true;
                    reset.IsEnabled = true;
                    break;
                case Game.colorOptions.p1Serves:
                    btnPlayer1.BackgroundColor = Color.Blue;
                    victory = false;
                    break;
                case Game.colorOptions.p2Serves:
                    btnPlayer2.BackgroundColor = Color.Blue;
                    victory = false;
                    break;
            }



            count = int.Parse(game.score1) + int.Parse(game.score2);

            if (count == 0) {
                PointsPicker.IsEnabled = true;
                singleServe = false;
                toggleServer.IsEnabled = true;
            }
            else {

                PointsPicker.IsEnabled = false;

                toggleServer.IsEnabled = false;

            }

            if (game.gameStopwatch != null && game.gameStopwatch.ElapsedTicks > 0) {
                stopwatch = game.gameStopwatch;
            }
            else {
                stopwatch = new Stopwatch();
            }



        }


        private void ChangeGamesToWin(object o, EventArgs e) {
            Picker caller = (Picker)o;
            currentTournament.gamesToWinIndex = caller.SelectedIndex;
        }

        private void LoadGames() {


            string temp = DependencyService.Get<ISaveAndLoad>().LoadText(SAVED_GAMES);

            if (temp != "") {
                games = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Game>>(temp);


                DisplayGames();
            }


        }

        private void DisplayGames() {
            newLbls.Children.Clear();
            if (games.Count != 0) {
                foreach (Game game in games) {
                    string log = game.ToString();
                    Label lbl = new Label() { Text = log, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) };
                    newLbls.Children.Add(lbl);
                }
            }
        }


        private void Reset() {
            btnPlayer1.Text = "0";
            btnPlayer2.Text = "0";
            btnPlayer1.IsEnabled = true;
            btnPlayer2.IsEnabled = true;
            btnPlayer1.BackgroundColor = Color.Blue;
            btnPlayer2.BackgroundColor = Color.Gray;
            PointsPicker.IsEnabled = true;
            count = 0;
            victory = false;
            singleServe = false;
            toggleServer.IsEnabled = true;

            stopwatch.Reset();



            history = new Stack<int>();

        }

        private void chkServer() {
            //count++;


            int score1 = int.Parse(btnPlayer1.Text);
            int score2 = int.Parse(btnPlayer2.Text);

            count = score1 + score2;


            if (singleServe) {
                changeServer();
            }
            else if (victoryScore == 11 && score1 == 10 && score2 == 10) {
                changeServer();
                singleServe = true;
            }
            else if (victoryScore == 11 && count % 2 == 0) {
                changeServer();
            }
            else if (victoryScore == 21 && count % 5 == 0) {

                changeServer();
            }
        }

        public void switchServer(object sender, EventArgs e) {
            changeServer();
        }

        public void changeServer() {
            if (btnPlayer1.BackgroundColor == Color.Blue) {
                btnPlayer1.BackgroundColor = Color.Gray;
                btnPlayer2.BackgroundColor = Color.Blue;
            }
            else {
                btnPlayer1.BackgroundColor = Color.Blue;
                btnPlayer2.BackgroundColor = Color.Gray;
            }
        }

        public void ResetBtn(object sender, EventArgs e) {
            Reset();
        }
        public void ChangeVictory(object sender, EventArgs e) {
            Picker num = (Picker)sender;
            victoryScore = int.Parse(PointsPicker.Items[PointsPicker.SelectedIndex]);
        }

        private void UndoVictory() {
            victory = false;
            btnPlayer1.IsEnabled = true;
            btnPlayer2.IsEnabled = true;
            if (btnPlayer1.BackgroundColor == Color.Blue) {
                btnPlayer2.BackgroundColor = Color.Gray;
            }
            else if (btnPlayer2.BackgroundColor == Color.Blue) {
                btnPlayer1.BackgroundColor = Color.Gray;
            }
            else if (btnPlayer1.BackgroundColor == Color.Green) {
                btnPlayer1.BackgroundColor = Color.Blue;
            }
            else if (btnPlayer2.BackgroundColor == Color.Green) {
                btnPlayer2.BackgroundColor = Color.Blue;
            }
        }

        //can't undo game what's already finished, since it's already logged as a victory and dealing with that is a hustle
        public void Undo(object o, EventArgs e) {
            if (history.Count > 0) {

                //bool undoingVictory = victory;
                if (victory) {
                    UndoGame();
                    UndoVictory();
                    DisplayGames();
                }
                else {
                    chkServer();
                }

                int prev = history.Pop();
                Button btn;
                if (prev == 1) {
                    btn = btnPlayer1;
                }
                else {
                    btn = btnPlayer2;

                }

                //if (undoingVictory) {
                //    chkServer();
                //}


                int val;
                int.TryParse(btn.Text, out val);
                val--;
                btn.Text = val.ToString();
                //count = count - 2;



                //int score1 = int.Parse(btnPlayer1.Text);
                //int score2 = int.Parse(btnPlayer2.Text);
                //if((score1+score2))
                if (history.Count == 0) {
                    btnUndo.IsEnabled = false;
                }

            }



        }


        public void Iterate(object sender, EventArgs e) {
            //make sure game is not already finished, green color buttons?
            if (btnPlayer1.BackgroundColor != Color.Green && btnPlayer2.BackgroundColor != Color.Green) {
                if (!stopwatch.IsRunning) {
                    stopwatch.Start();
                }


                btnUndo.IsEnabled = true;
                PointsPicker.IsEnabled = false;
                toggleServer.IsEnabled = false;
                Button btn = (Button)sender;


                if (btn == btnPlayer1) {
                    history.Push(1);
                }
                else {
                    history.Push(2);
                }




                int score = int.Parse(btn.Text);
                score++;
                btn.Text = (score).ToString();
                if (score >= victoryScore) {
                    checkVictory();
                }
                if (!victory) {
                    chkServer();
                }
            }



        }

        private void checkVictory() {
            int p1 = int.Parse(btnPlayer1.Text);
            int p2 = int.Parse(btnPlayer2.Text);
            string victor = "";
            if (Math.Abs(p1 - p2) >= 2) {
                btnPlayer1.IsEnabled = false;
                btnPlayer2.IsEnabled = false;
                stopwatch.Stop();

                if (p1 > p2) {
                    btnPlayer1.BackgroundColor = Color.Green;
                    //victor = player1.Text;

                    currentTournament.p1GamesWon++;
                }
                else {
                    btnPlayer2.BackgroundColor = Color.Green;
                    //victor = player2.Text;

                    currentTournament.p2GamesWon++;
                }


                


                victory = true;
                reset.IsEnabled = true;
                //btnUndo.IsEnabled = false;
                logGame();
                DisplayGames();



                ////////////////////////////////////
                //this can be placed in an if statement when doing multiple games per round
                int gamesPlayed = currentTournament.p1GamesWon + currentTournament.p2GamesWon;
                int gamesToPlay = currentTournament.gamesToWin[currentTournament.gamesToWinIndex];


                if (gamesToPlay == gamesPlayed) {

                    if (currentTournament.p1GamesWon > currentTournament.p2GamesWon) {
                        victor = player1.Text;
                    }
                    else {
                        victor = player2.Text;
                    }


                    if (currentTournament != null) {

                        currentTournament.AdvancePlayerToNextRound(new Participant { Name = victor });
                    }

                    if (tournamentGame && currentTournament != null && currentTournament.VictorDecided()) {
                        reset.IsEnabled = false;
                        DisplayAlert("Champion", "The champion of this tournament is " + victor + "!", "OK");
                    }
                }
                
                /////////////////////////////////////////////////////////////////////
            }

        }

        private void logGame() {


            Game thisGame = new Game();
            thisGame.player1 = player1.Text;
            thisGame.player2 = player2.Text;
            thisGame.score1 = btnPlayer1.Text;
            thisGame.score2 = btnPlayer2.Text;
            thisGame.timeMin = stopwatch.Elapsed.Minutes.ToString();
            thisGame.timeSec = stopwatch.Elapsed.Seconds.ToString();
            thisGame.gameStopwatch = stopwatch;

            games.Insert(0, thisGame);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(games);

            DependencyService.Get<ISaveAndLoad>().SaveText(SAVED_GAMES, json.ToString());

        }

        private void UndoGame() {
            games.RemoveAt(0);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(games);

            DependencyService.Get<ISaveAndLoad>().SaveText(SAVED_GAMES, json.ToString());
        }

        public void ClearRecords(object sender, EventArgs e) {
            ClearRecords();

        }
        public void ClearRecords() {
            DependencyService.Get<ISaveAndLoad>().SaveText(SAVED_GAMES, "");
            games.Clear();
            DisplayGames();

        }

    }
}
