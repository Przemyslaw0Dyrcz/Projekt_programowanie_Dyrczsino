namespace Dyrczsino
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage
            {
                BindingContext = new MainViewModel()
            };
        }
    }
}
