using System;
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
        private int count { get; set; }
        private bool victory { get; set; }
        private bool singleServe { get; set; }
        private Stopwatch stopwatch;
        private List<Game> games = new List<Game>();
        private string SAVED_GAMES ;
        public Tournament currentTournament;
        public bool tournamentGame;
        public Stack<int> history;

        public MainPage() {
            Title = "Single Match";
            SAVED_GAMES ="match_log.txt";
            tournamentGame = false;

            InitializeComponent();
            PointsPicker.SelectedIndex = 1;
            stopwatch = new Stopwatch();
            player1.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
            player2.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));


            reset.Clicked += ResetBtn;

            reset.IsEnabled = true;




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

            if (btnPlayer1.BackgroundColor == Color.Blue) {
                thisGame.colorSetup = Game.colorOptions.p1Serves;
            }
            else if (btnPlayer2.BackgroundColor == Color.Blue) {
                thisGame.colorSetup = Game.colorOptions.p2Serves;
            }
            else if (btnPlayer1.BackgroundColor == Color.Green) {
                thisGame.colorSetup = Game.colorOptions.p1Won;
            }
            else if (btnPlayer2.BackgroundColor == Color.Green) {
                thisGame.colorSetup = Game.colorOptions.p2Won;
            }
            return thisGame;
        }

        public MainPage(Tournament tournament) {
            Title = "Tournament Match";
            SAVED_GAMES = "tournament_log.txt";
            tournamentGame = true;
            


            InitializeComponent();
            player1.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
            player2.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));


            currentTournament = tournament;
            reset.Clicked += setUpNextTournamentGame;
            reset.IsEnabled = false;

            SetUpGame(tournament.NextGame());
            ClearRecords();


            LoadGames();


        }
        public MainPage(Tournament tournament, Game game) {
            Title = "Tournament Match";
            SAVED_GAMES = "tournament_log.txt";
            tournamentGame = true;



            InitializeComponent();
            player1.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
            player2.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));


            currentTournament = tournament;
            reset.Clicked += setUpNextTournamentGame;
            reset.IsEnabled = false;

            SetUpGame(game);
            ClearRecords();


            LoadGames();


        }

        public void setUpNextTournamentGame(object o, EventArgs e) {
            if (!currentTournament.VictorDecided()) {

                SetUpGame(currentTournament.NextGame());
            }
        }

        public void SetUpGame(Game game) {
            player1.Text = game.player1;
            player1.IsEnabled = false;
            player2.Text = game.player2;
            player2.IsEnabled = false;
            btnPlayer1.Text = game.score1;
            btnPlayer2.Text = game.score2;
            PointsPicker.SelectedIndex = game.victoryScoreIndex;
            singleServe = game.singleServe;


            history = game.history;


            btnPlayer1.BackgroundColor = Color.Gray;
            btnPlayer1.BackgroundColor = Color.Gray;

            switch (game.colorSetup) {
                case Game.colorOptions.p1Serves:
                    btnPlayer1.BackgroundColor = Color.Blue;
                    victory = false;
                    break;
                case Game.colorOptions.p2Serves:
                    btnPlayer2.BackgroundColor = Color.Blue;
                    victory = false;
                    break;
                case Game.colorOptions.p1Won:
                    btnPlayer1.BackgroundColor = Color.Green;
                    victory = true;
                    break;
                case Game.colorOptions.p2Won:
                    btnPlayer1.BackgroundColor = Color.Green;
                    victory = true;
                    break;
            }



            count = int.Parse(game.score1) + int.Parse(game.score2);

            if(count == 0) {
                PointsPicker.IsEnabled = true;
                singleServe = false;
                toggleServer.IsEnabled = true;
            }
            else {

                PointsPicker.IsEnabled = false;

                toggleServer.IsEnabled = false;

            }

            if(game.gameStopwatch != null && game.gameStopwatch.ElapsedTicks > 0) {
                stopwatch = game.gameStopwatch;
            }
            else {
                stopwatch = new Stopwatch();
            }



        }
        

        private void LoadGames() {
            
            
            string  temp = DependencyService.Get<ISaveAndLoad>().LoadText(SAVED_GAMES);

            if(temp!= "") {
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
            count++;


            int score1 = int.Parse(btnPlayer1.Text);
            int score2 = int.Parse(btnPlayer2.Text);

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

        

        public void Iterate(object sender, EventArgs e) {
            if (!stopwatch.IsRunning) {
                stopwatch.Start();
            }

            PointsPicker.IsEnabled = false;
            toggleServer.IsEnabled = false;
            Button btn = (Button)sender;
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
                    victor = player1.Text;
                }
                else {
                    btnPlayer2.BackgroundColor = Color.Green;
                    victor = player2.Text;
                }


                if(currentTournament != null) {

                    currentTournament.AdvancePlayerToNextRound(new Participant { Name = victor });
                }


                victory = true;
                reset.IsEnabled = true;
                logGame();
                DisplayGames();

                if (tournamentGame && currentTournament!= null && currentTournament.VictorDecided()) {
                    reset.IsEnabled = false;
                    DisplayAlert("Champion", "The champion of this tournament is "+victor+"!", "OK");
                }

            }

        }

        private void logGame() {
            

            Game thisGame = new Game();
            thisGame.player1 = player1.Text;
            thisGame.player2 = player2.Text;
            thisGame.score1 = btnPlayer1.Text;
            thisGame.score2 = btnPlayer2.Text;
            thisGame.timeMin= stopwatch.Elapsed.Minutes.ToString();
            thisGame.timeSec = stopwatch.Elapsed.Seconds.ToString();
            thisGame.gameStopwatch = stopwatch;
            
            games.Insert(0, thisGame);
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
