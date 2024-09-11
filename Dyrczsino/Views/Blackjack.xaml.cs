using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;


namespace Dyrczsino.Views
{
    public partial class Blackjack : ContentView
    {
        private List<string> deck;
        private List<string> playerCards;
        private List<string> croupierCards;
        private int playerPoints = 0;
        private int croupierPoints = 0;
        private Random random = new Random();
        private int currentRuleIndex = 0;
        public Blackjack()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            deck = CreateDeck();
            ShuffleDeck();
            playerCards = new List<string>();
            croupierCards = new List<string>();

            playerPoints = 0;
            croupierPoints = 0;
            PlayerPointsLabel.Text = "0";
            CroupierPointsLabel.Text = "0";

            PlayerGrid.Children.Clear();
            CroupierGrid.Children.Clear();

            PlayerGrid.ColumnDefinitions.Clear();
            CroupierGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 10; i++)
            {
                PlayerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                CroupierGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            GameStatusLabel.Text = "Good luck!";
            GameStatusLabel.TextColor = Colors.Green;
        }

        private List<string> CreateDeck()
        {
            var suits = new[] { "♥", "♦", "♣", "♠" };
            var values = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            var deck = new List<string>();

            foreach (var suit in suits)
            {
                foreach (var value in values)
                {
                    deck.Add($"{value}{suit}");
                }
            }
            return deck;
        }

        private void ShuffleDeck()
        {
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = deck[i];
                deck[i] = deck[j];
                deck[j] = temp;
            }
        }

        private void AddCardToGrid(Grid grid, string card, int index)
        {
            var cardLabel = new Frame
            {
                Content = new Label
                {
                    Text = card,
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 12,
                    Margin = new Thickness(0)
                },
                WidthRequest = 60,
                HeightRequest = 90,
                BorderColor = Colors.Black,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            if (index >= grid.Children.Count)
            {
                grid.Children.Add(cardLabel);
                Grid.SetColumn(cardLabel, index);
            }
            else
            {
                grid.Children.RemoveAt(index);
                grid.Children.Insert(index, cardLabel);
                Grid.SetColumn(cardLabel, index);
            }
        }

        private void UpdatePlayerPoints(string card)
        {
            string value = card.Substring(0, card.Length - 1);
            int points = 0;

            switch (value)
            {
                case "A":
                    points = 11;
                    break;
                case "K":
                case "Q":
                case "J":
                    points = 10;
                    break;
                default:
                    points = int.Parse(value);
                    break;
            }

            playerPoints += points;
            PlayerPointsLabel.Text = playerPoints.ToString();

            if (playerPoints > 21)
            {
                GameStatusLabel.Text = "Busted! You lose!";
                GameStatusLabel.TextColor = Colors.Red;
            }
        }

        private void UpdateCroupierPoints(string card)
        {
            string value = card.Substring(0, card.Length - 1);
            int points = 0;

            switch (value)
            {
                case "A":
                    points = 11;
                    break;
                case "K":
                case "Q":
                case "J":
                    points = 10;
                    break;
                default:
                    points = int.Parse(value);
                    break;
            }

            croupierPoints += points;
            CroupierPointsLabel.Text = croupierPoints.ToString();
        }

        private void OnHitButtonClicked(object sender, EventArgs e)
        {
            if (deck.Count > 0 && playerCards.Count < 10)
            {
                string card = DrawCard();
                playerCards.Add(card);
                AddCardToGrid(PlayerGrid, card, playerCards.Count - 1);
                UpdatePlayerPoints(card);

                if (playerPoints > 21)
                {
                    GameStatusLabel.Text = "Busted! You lose!";
                    GameStatusLabel.TextColor = Colors.Red;
                    HitButton.IsEnabled = false;
                }
            }
        }
        private void OnQuestionMarkClicked(object sender, EventArgs e)
        {
            QuestionMarkButton.IsVisible = false;
            GameRulesScrollView.IsVisible = true;
            NextButton.IsVisible = true;
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            var rules = new[] { GameRule1, GameRule2, GameRule3, GameRule4, GameRule5 };

            foreach (var rule in rules)
            {
                rule.IsVisible = false;
            }

            if (currentRuleIndex < rules.Length - 1)
            {
                currentRuleIndex++;
                rules[currentRuleIndex].IsVisible = true;
            }
            else
            {
                GameRulesScrollView.IsVisible = false;
                QuestionMarkButton.IsVisible = true;
                NextButton.IsVisible = false;
                currentRuleIndex = 0;
            }
        }
        private void OnStandButtonClicked(object sender, EventArgs e)
        {
            while (croupierPoints < 17 && deck.Count > 0)
            {
                string card = DrawCard();
                croupierCards.Add(card);
                AddCardToGrid(CroupierGrid, card, croupierCards.Count - 1);
                UpdateCroupierPoints(card);
            }

            if (playerPoints > 21 && croupierPoints > 21)
            {
                GameStatusLabel.Text = "Both busted! It's a tie!";
                GameStatusLabel.TextColor = Colors.Gray;
            }
            else if (playerPoints > 21)
            {
                GameStatusLabel.Text = "Busted! You lose!";
                GameStatusLabel.TextColor = Colors.Red;
                
            }
            else if (croupierPoints > 21)
            {
                GameStatusLabel.Text = "Croupier busted! You win!";
                GameStatusLabel.TextColor = Colors.Green;
            }
            else
            {
                if (playerPoints > croupierPoints)
                {
                    GameStatusLabel.Text = "You win!";
                    GameStatusLabel.TextColor = Colors.Green;
                }
                else if (playerPoints < croupierPoints)
                {
                    GameStatusLabel.Text = "You lose!";
                    GameStatusLabel.TextColor = Colors.Red;
                }
                else
                {
                    GameStatusLabel.Text = "It's a tie!";
                    GameStatusLabel.TextColor = Colors.Gray;
                }
            }
        }

        private void OnRestartButtonClicked(object sender, EventArgs e)
        {
            HitButton.IsEnabled = true;
            InitializeGame();
        }

        private string DrawCard()
        {
            string card = deck[0];
            deck.RemoveAt(0);
            return card;
        }
    }
}
