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
        private static Stopwatch stopwatch;
        private static List<Game> games = new List<Game>();
        private const string SAVED_GAMES = "temp.txt";

        public MainPage() {


            
            InitializeComponent();
            PointsPicker.SelectedIndex = 1;
            stopwatch = new Stopwatch();
            Reset();


            LoadGames(SAVED_GAMES);


        }


        //LoadGames() without parameter will probably work with database in the future, not yet implemented
        private void LoadGames(string file) {
            

            //lblTime.Text= DependencyService.Get<ISaveAndLoad>().LoadText("temp.txt");
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(thisGame);
            newLbls.Children.Clear();
            string  temp = DependencyService.Get<ISaveAndLoad>().LoadText(SAVED_GAMES);

            if(temp!= "") {
                games = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Game>>(temp);

                games = games.OrderByDescending(x => x.timeStamp).ToList();

                foreach (Game game in games) {
                    string log = game.ToString();
                    Label lbl = new Label() { Text = log, HorizontalOptions = LayoutOptions.FillAndExpand };
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
            if (Math.Abs(p1 - p2) >= 2) {
                btnPlayer1.IsEnabled = false;
                btnPlayer2.IsEnabled = false;
                stopwatch.Stop();
                lblTime.Text = stopwatch.Elapsed.ToString();

                if (p1 > p2) {
                    btnPlayer1.BackgroundColor = Color.Green;
                }
                else {
                    btnPlayer2.BackgroundColor = Color.Green;
                }
                victory = true;

                logGame();
                LoadGames(SAVED_GAMES);

            }

        }

        private void logGame() {
            //string plr1 = player1.Text;
            //string plr2 = player2.Text;
            //string scr1 = btnPlayer1.Text;
            //string scr2 = btnPlayer2.Text;
            //string timeMin = stopwatch.Elapsed.Minutes.ToString();
            //string timeSec = stopwatch.Elapsed.Seconds.ToString();

            //string log = plr1 + " vs. " + plr2 + " <" + scr1 + ":" + scr2 +
            //    "> Time: " + timeMin + " min. " + timeSec + " sec.";

            ///StackLayout stack = Content.FindByName<StackLayout>("newLbls");
            ///

            Game thisGame = new Game();
            thisGame.player1 = player1.Text;
            thisGame.player2 = player2.Text;
            thisGame.score1 = btnPlayer1.Text;
            thisGame.score2 = btnPlayer2.Text;
            thisGame.timeMin= stopwatch.Elapsed.Minutes.ToString();
            thisGame.timeSec = stopwatch.Elapsed.Seconds.ToString();

            //string log = thisGame.ToString();
            //Label lbl = new Label() { Text = log, HorizontalOptions = LayoutOptions.FillAndExpand };
            //newLbls.Children.Add(lbl);

            games.Insert(0, thisGame);
            //games.Add(thisGame);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(games);

            DependencyService.Get<ISaveAndLoad>().SaveText(SAVED_GAMES, json.ToString());

            //this.Resources
            //Game game = new Game(Resources);


        }

        public void ClearRecords(object sender, EventArgs e) {
            DependencyService.Get<ISaveAndLoad>().SaveText(SAVED_GAMES, "");
            games.Clear();
            LoadGames(SAVED_GAMES);

        }


    }
}
