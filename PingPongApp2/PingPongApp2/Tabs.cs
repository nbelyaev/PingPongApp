using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PingPongApp2 {
    public class Tabs : TabbedPage {
        private ParticipantsPage partPage;
        private MainPage gamePage;
        public Tabs() {
            Title = "Tabs";

            gamePage = new MainPage();
            NavigationPage main = new NavigationPage(gamePage);
            main.Title = "Match";
            Children.Add(main);

            partPage = new ParticipantsPage();
            NavigationPage tournament = new NavigationPage(partPage);
            tournament.Title = "Tournament";
            Children.Add(tournament);

            tournament.Popped += PreserveGame;

        }

        public void PreserveGame(object o, EventArgs e) {
            if (partPage.currentGamePage.victory&& partPage.currentTournament.VictorDecided()) {
            //if (partPage.currentTournament.VictorDecided()) {
                partPage.contunueGame.IsEnabled = false;
                partPage.currentTournament = null;
                partPage.currentGame = null;
                //partPage.stackParticipants.IsEnabled = true;
                //List<View> list = partPage.stackParticipants.Children.ToList();
                //foreach (View v in list) {
                //    v.IsEnabled = true;
                //}

            }
            else {
                partPage.currentGame = partPage.currentGamePage.SaveGame();
                partPage.contunueGame.IsEnabled = true;
                //List<View> list = partPage.stackParticipants.Children.ToList();
                
                //foreach(View v in list) {
                //    v.IsEnabled = false;
                //}

            }
        }
    }
}
