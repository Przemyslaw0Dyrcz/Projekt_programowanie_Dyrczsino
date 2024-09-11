using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Dyrczsino
{
    public class MainViewModel : BindableObject
    {
        public ICommand SelectViewCommand { get; }

        public MainViewModel()
        {
            SelectViewCommand = new Command<string>(OnSelectView);
        }

        private void OnSelectView(string viewName)
        {
            if (Application.Current.MainPage is MainPage mainPage)
            {
                mainPage.LoadViewAsync(viewName);
            }
        }
    }
}
