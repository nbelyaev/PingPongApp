using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using UIKit;
using Xamarin.Forms;

namespace PingPongApp2 {
    public class ParticipantsPage : ContentPage {


        //readonly Dictionary<string, object> _names = new Dictionary<string, object>();

        //object FindByName(string name) {
        //    if (_names.ContainsKey(name))
        //        return _names[name];
        //    return null;
        //}

        //void RegisterName(string name, object scopedElement) {
        //    if (_names.ContainsKey(name))
        //        throw new ArgumentException("An element with the same key already exists in NameScope", "name");

        //    _names[name] = scopedElement;
        //}

        private static List<Participant> participants = new List<Participant>();
        private const string LIST_OF_PARTICIPANTS = "participants.txt";
        private static Entry nameEntry;
        private static StackLayout stackParticipants;
        public Game currentGame;
        public Button contunueGame;
        public MainPage currentGamePage;

        public ParticipantsPage() {

            Title = "Tournament";
            //Content = new StackLayout {
            //    Children = {
            //        new Label { Text = "List of participants and relevant controls" },
            //        new Button { Text="NEW TOURNAMENT", ClassId="btn" },
            //         new Button { Text="ADD NEW PARTICIPANT", ClassId="btn1" }
            //    }
            //};
            StackLayout pageContent = new StackLayout();
            //Label lbl = new Label { Text = "hoho" };

            Content = pageContent;
            //pageContent.Children.Add(lbl);

            Initialize();

            LoadParticipants();

            

            //Button btnNewTourn = new Button { Text = "NEW TOURNAMENT", ClassId = "btn" };
            //Entry newParticipant = new Entry { Placeholder = "Name of a new participant",
            //    HorizontalOptions =LayoutOptions.FillAndExpand, HorizontalTextAlignment=TextAlignment.Center,
            //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry))
            //};
            //Button btnNewPart = new Button { Text = "ADD NEW PARTICIPANT", ClassId = "btn1"};
            //btnNewPart.Clicked += AddParticipant;

            //pageContent.Children.Add(btnNewTourn);
            //pageContent.Children.Add(newParticipant);
            //pageContent.Children.Add(btnNewPart);
            //StackLayout participants = new StackLayout ();
            //pageContent.Children.Add(participants);


        }

        
        private void LoadParticipants() {
            string temp = DependencyService.Get<ISaveAndLoad>().LoadText(LIST_OF_PARTICIPANTS);

            if (temp != "") {
                participants = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Participant>>(temp);

                //games = games.OrderByDescending(x => x.timeStamp).ToList();

                //foreach (Game game in games) {
                //    string log = game.ToString();
                //    Label lbl = new Label() { Text = log, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) };
                //    newLbls.Children.Add(lbl);
                //}


                DisplayParticipants();
            }
        }

        private void DisplayParticipants() {
            //StackLayout stackParticipants = Content.FindByName<StackLayout>("stackParticipants");
            stackParticipants.Children.Clear();
            if (participants.Count != 0) {
                foreach (Participant part in participants) {
                    Button remove = new Button { Text = "X" , HorizontalOptions= LayoutOptions.End};
                    remove.Clicked += RemoveName;


                    Label lbl = new Label() { Text = part.Name, HorizontalOptions = LayoutOptions.FillAndExpand,
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
            StackLayout stack= (StackLayout)((Button)o).Parent;
            stackParticipants.Children.Remove(stack);
        }

        private void Initialize() {

            //StackLayout pageContent = Content.FindByName<StackLayout>("pageContent");
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
                currentGame = new Game();
                currentGamePage = new MainPage(currentGame);
                await Navigation.PushAsync(currentGamePage);
            }
            else {
                
                await DisplayAlert("Alert", "You need at least 2 participants!", "OK");
            }
            
        }

        private async void ContinueTournament(object o, EventArgs e) {
            //currentGame =  currentGame;
            await Navigation.PushAsync(new MainPage(currentGame));
        }


        public void AddParticipant(object sender, EventArgs e) {
            //StackLayout pageContent = (StackLayout)Content;
            //pageContent.Children.ToDictionary;
            //Entry entry = (Entry)pageContent.FindByName<object>("newParticipant");
            //Entry entry = pageContent.Children.
            if (nameEntry.Text != "" && nameEntry.Text != null) {
                Participant participant = new Participant(nameEntry.Text);
                participants.Add(participant);
                nameEntry.Text = "";

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(participants);

                DependencyService.Get<ISaveAndLoad>().SaveText(LIST_OF_PARTICIPANTS, json.ToString());

                DisplayParticipants();

            }
            else {
                DisplayAlert("Alert", "You need to enter a name!", "OK");
            }

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(games);

            //DependencyService.Get<ISaveAndLoad>().SaveText(SAVED_GAMES, json.ToString());
        }
        
    }
}
