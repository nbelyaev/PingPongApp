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
            ///Children.Add(new MainPage());
            //Children.Add(new NavigationPage( new ParticipantsPage() ));

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
            partPage.currentGame = partPage.currentGamePage.SaveGame();
            partPage.contunueGame.IsEnabled = true;
        }
    }
}
