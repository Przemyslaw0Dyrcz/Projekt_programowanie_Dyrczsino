using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Dyrczsino.Views
{
    public partial class Roulette : ContentView
    {
        private int stake = 1;
        private string selectedColor = null;
        private List<string> colors;
        private System.Timers.Timer rotationTimer;
        private int currentIndex = 0;
        private string winningColor;
        private int winningIndex;
        private int currentRuleIndex = 0;

        public Roulette()
        {
            InitializeComponent();

       
            colors = GenerateColorSequence();

           
            rotationTimer = new System.Timers.Timer(500);
            rotationTimer.Elapsed += OnRotationTimerElapsed;
        }

        private List<string> GenerateColorSequence()
        {
            var sequence = new List<string>();
            for (int i = 0; i < 36; i++)
            {
                sequence.Add((i % 2 == 0) ? "Czerwony" : "Czarny");
            }

            var random = new Random();
            int greenPosition = random.Next(0, 37);
            sequence.Insert(greenPosition, "Zielony");

            return sequence;
        }

        private void DecreaseStake_Clicked(object sender, EventArgs e)
        {
            if (stake > 1)
            {
                stake--;
                StakeLabel.Text = stake.ToString();
            }
        }

        private void IncreaseStake_Clicked(object sender, EventArgs e)
        {
            stake++;
            StakeLabel.Text = stake.ToString();
        }

        private void OnColorSelected(object sender, EventArgs e)
        {
            var button = (Button)sender;

            RedButton.BorderColor = Colors.Transparent;
            BlackButton.BorderColor = Colors.Transparent;
            GreenButton.BorderColor = Colors.Transparent;

            button.BorderColor = Colors.Gold;
            button.BorderWidth = 3;

            if (button.BackgroundColor == Colors.Red)
                selectedColor = "Czerwony";
            else if (button.BackgroundColor == Colors.Black)
                selectedColor = "Czarny";
            else if (button.BackgroundColor == Colors.Green)
                selectedColor = "Zielony";
        }

        private void PlayRoulette(object sender, EventArgs e)
        {
            if (selectedColor == null)
            {
                ResultLabel.Text = "Wybierz kolor!";
                return;
            }

            colors = GenerateColorSequence();

            var random = new Random();
            winningIndex = random.Next(0, colors.Count);
            winningColor = colors[winningIndex];

            currentIndex = 0;
            rotationTimer.Start();

            ResultLabel.Text = "Losowanie...";
        }

        private void OnRotationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Dispatch(() =>
            {
                currentIndex = (currentIndex + 1) % colors.Count;

                string[] displayColors = new string[5];
                for (int i = 0; i < 5; i++)
                {
                    int index = (currentIndex + i) % colors.Count;
                    displayColors[i] = colors[index];
                }

                SetSquareColor(Square1, displayColors[0]);
                SetSquareColor(Square2, displayColors[1]);
                SetSquareColor(Square3, displayColors[2]);
                SetSquareColor(Square4, displayColors[3]);
                SetSquareColor(Square5, displayColors[4]);

                if ((currentIndex + 2) % colors.Count == winningIndex)
                {
                    rotationTimer.Stop();
                    ShowResult();
                }
            });
        }

        private void OnQuestionMarkClicked(object sender, EventArgs e)
        {
            QuestionMarkButton.IsVisible = false;
            GameRulesScrollView.IsVisible = true;
            NextButton.IsVisible = true;
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            var rules = new[] { GameRule1, GameRule2, GameRule3, GameRule4, GameRule5, GameRule6 };

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

        private void SetSquareColor(Frame square, string colorName)
        {
            switch (colorName)
            {
                case "Czerwony":
                    square.BackgroundColor = Colors.Red;
                    break;
                case "Czarny":
                    square.BackgroundColor = Colors.Black;
                    break;
                case "Zielony":
                    square.BackgroundColor = Colors.Green;
                    break;
                default:
                    square.BackgroundColor = Colors.Gray;
                    break;
            }
        }

        private void ShowResult()
        {
            int multiplier = selectedColor == winningColor ? (selectedColor == "Zielony" ? 8 : 2) : 0;
            double winnings = stake * multiplier;

            ResultLabel.Text = (multiplier > 0)
                ? $"Wylosowano: {winningColor} \nWygra³eœ: {winnings}!"
                : $"Wylosowano: {winningColor} \nNiestety, przegra³eœ.";

            ResultLabel.FontSize = 26;
            ResultLabel.FontAttributes = FontAttributes.Bold;
            ResultLabel.TextColor = Colors.Gold;
        }
    }
}
