using System;
using System.Timers;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Dyrczsino.Views
{
    public partial class Crash : ContentView
    {
        private double multiplier = 1.00;
        private double stake = 1;
        private bool isRunning = false;
        private bool isGameOver = false;
        private System.Timers.Timer timer;
        private double crashTime;
        private double increment = 0.02;
        private int elapsedTimeInSeconds = 0;
        private Random random;
        private int currentRuleIndex = 0;

        public Crash()
        {
            InitializeComponent();
            random = new Random();
            UpdateUI();

            timer = new System.Timers.Timer(250);
            timer.Elapsed += OnTimerElapsed;
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

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            elapsedTimeInSeconds += 1; 

            if (elapsedTimeInSeconds % 4 == 0)
            {
                increment += 0.01; 
            }


            if (multiplier >= crashTime)
            {
                Dispatcher.Dispatch(() => StopGame(false));
            }
            else
            {
                multiplier += increment;
                Dispatcher.Dispatch(() => UpdateUI());
            }
        }

        private void UpdateUI()
        {
            MultiplierLabel.Text = $"x{multiplier:F2}";
            StakeLabel.Text = $"{stake}";
            StartStopButton.Text = isRunning ? "Stop" : (isGameOver ? "Reset" : "Start");
        }

        private void StartStopButton_Clicked(object sender, EventArgs e)
        {
            if (isGameOver)
            {

                ResetGame();
            }
            else if (!isRunning)
            {

                isRunning = true;
                multiplier = 1.00;
                increment = 0.02; 
                elapsedTimeInSeconds = 0;
                crashTime = random.NextDouble() * 10 + 1;

                StatusLabel.Text = "";
                timer.Start();
                UpdateUI();
            }
            else
            {
                StopGame(true);
            }
        }

        private void StopGame(bool userStopped)
        {
            isRunning = false;
            isGameOver = true;
            timer.Stop();

            if (userStopped)
            {

                double winnings = stake * multiplier;

                StatusLabel.Text = $"You won ${winnings:F2}!";
            }
            else
            {

                StatusLabel.Text = "You lost! Try again!"; 
            }

            UpdateUI();
        }

        private void ResetGame()
        {
            isGameOver = false;

            StatusLabel.Text = ""; 
            UpdateUI(); 
        }

        private void IncreaseStake_Clicked(object sender, EventArgs e)
        {
            stake++;
            UpdateUI();
        }

        private void DecreaseStake_Clicked(object sender, EventArgs e)
        {
            if (stake > 1)
                stake--;
            UpdateUI();
        }
    }
}
