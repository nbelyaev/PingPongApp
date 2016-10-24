using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using UIKit;
using Xamarin.Forms;

namespace PingPongApp2 {
    public class ParticipantsPage : ContentPage {

        

        private static List<Participant> participants = new List<Participant>();
        private const string LIST_OF_PARTICIPANTS = "participants.txt";
        private static Entry nameEntry;
        public  StackLayout stackParticipants;
        public Game currentGame;
        public Button contunueGame;
        public MainPage currentGamePage;
        public Tournament currentTournament;

        public ParticipantsPage() {

            Title = "Tournament";
            
            StackLayout pageContent = new StackLayout();
            Content = pageContent;
            Initialize();

            LoadParticipants();


            DisplayParticipants();


        }

        
        private void LoadParticipants() {
            string temp = DependencyService.Get<ISaveAndLoad>().LoadText(LIST_OF_PARTICIPANTS);

            if (temp != "") {
                participants = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Participant>>(temp);
                
            }
        }

        private void DisplayParticipants() {
            stackParticipants.Children.Clear();
            if (participants.Count != 0) {
                foreach (Participant part in participants) {
                    Button remove = new Button { Text = "X" , HorizontalOptions= LayoutOptions.End, CommandParameter=part};
                    remove.Clicked += RemoveName;


                    Label lbl = new Label() { Text = part.Name,  HorizontalOptions = LayoutOptions.FillAndExpand,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) };

                    StackLayout nameStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand ,
                        Orientation= StackOrientation.Horizontal};
                    nameStack.Children.Add(lbl);
                    nameStack.Children.Add(remove);


                    stackParticipants.Children.Add(nameStack);
                }
            }
        }

        private void RemoveName(object o, EventArgs e) {
            Button callerBtn = (Button)o;
            Participant playerToRemove = (Participant)callerBtn.CommandParameter;

            if (currentGame == null || currentTournament.RemoveParticipant(playerToRemove)) {
                StackLayout stack = (StackLayout)callerBtn.Parent;
                stackParticipants.Children.Remove(stack);
                //Label lbl= stack.FindByName<Label>("lbl");
                //int n =((StackLayout)(stack.Parent)).Children.IndexOf(stack);
                //((Button)o).CommandParameter.ToString();
                participants.Remove(playerToRemove);
                SaveParticipants();
            }
            else {
                DisplayAlert("Unable to remove", "You can only remove players what are not currently playing", "OK");
            }

        }

        private void Initialize() {
            
            StackLayout pageContent = (StackLayout)Content;


            StackLayout tournamentOptions = new StackLayout { Orientation= StackOrientation.Horizontal, Padding= new Thickness(0,25) };
            Button btnNewTourn = new Button { Text = "NEW TOURNAMENT",  HeightRequest=100, HorizontalOptions=LayoutOptions.FillAndExpand };
            btnNewTourn.Clicked += StartNewTournament;
            contunueGame = new Button { Text = "CONTINUE", HeightRequest = 100, HorizontalOptions = LayoutOptions.FillAndExpand
                , IsEnabled=false };
            contunueGame.Clicked += ContinueTournament;


            tournamentOptions.Children.Add(btnNewTourn);
            tournamentOptions.Children.Add(contunueGame);

            Entry newParticipant = new Entry {
                Placeholder = "Name of a new participant",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry))
            };

            nameEntry = newParticipant;

            Button btnNewPart = new Button { Text = "ADD NEW PARTICIPANT", ClassId = "btn1" };
            btnNewPart.Clicked += AddParticipant;

            pageContent.Children.Add(tournamentOptions);
            pageContent.Children.Add(newParticipant);
            pageContent.Children.Add(btnNewPart);
            stackParticipants = new StackLayout();
            pageContent.Children.Add(stackParticipants);
        }

        private async void StartNewTournament(object o, EventArgs e) {
            if(participants!=null && participants.Count > 1) {
                //currentGame = new Game();
                currentTournament = new Tournament(participants);
                //currentGame = currentTournament.NextGame();
                
                currentGamePage = new MainPage(currentTournament);
                await Navigation.PushAsync(currentGamePage);
            }
            else {
                
                await DisplayAlert("Alert", "You need at least 2 participants!", "OK");
            }
            
        }

        private async void ContinueTournament(object o, EventArgs e) {

            currentGamePage = new MainPage(currentTournament, currentGame);
            await Navigation.PushAsync(currentGamePage);
        }


        public void AddParticipant(object sender, EventArgs e) {
            
            if (nameEntry.Text != "" && nameEntry.Text != null) {
                Participant participant = new Participant(nameEntry.Text);
                participants.Add(participant);
                nameEntry.Text = "";

                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(participants);

                //DependencyService.Get<ISaveAndLoad>().SaveText(LIST_OF_PARTICIPANTS, json.ToString());
                SaveParticipants();


                DisplayParticipants();

                if(currentTournament != null) {
                    currentTournament.AddParticipant(participant);
                }


            }
            else {
                DisplayAlert("Alert", "You need to enter a name!", "OK");
            }
            
        }

        private void SaveParticipants() {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(participants);

            DependencyService.Get<ISaveAndLoad>().SaveText(LIST_OF_PARTICIPANTS, json.ToString());
        }
        
    }
}
