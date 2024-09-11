using Microsoft.Maui.Controls;
using Dyrczsino.Views;
using System;
using System.Threading.Tasks;

namespace Dyrczsino
{
    public partial class MainPage : ContentPage
    {
        private bool isSidebarExpanded = false;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(); 
        }

        private void OnExpandButtonClicked(object sender, EventArgs e)
        {

            if (isSidebarExpanded)
            {
                Sidebar.IsVisible = false;
                ExpandButton.Text = "☰"; 
            }
            else
            {
                Sidebar.IsVisible = true;
                ExpandButton.Text = "×"; 
            }

            isSidebarExpanded = !isSidebarExpanded;
        }

        public void LoadViewAsync(string viewName)
        {

            ContentView.Content = null;


            switch (viewName)
            {
                case "Blackjack":
                    ContentView.Content = new Blackjack();
                    break;
                case "Crash":
                    ContentView.Content = new Crash();
                    break;
                case "Roulette":
                    ContentView.Content = new Roulette();
                    break;
                case "History":
                    ContentView.Content = new History();
                    break;
            }

            if (isSidebarExpanded)
            {
                OnExpandButtonClicked(null, EventArgs.Empty);
            }
        }
    }
}
