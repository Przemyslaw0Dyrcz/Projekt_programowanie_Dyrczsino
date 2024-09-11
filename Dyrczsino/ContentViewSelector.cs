using Microsoft.Maui.Controls;

namespace Dyrczsino
{
    public class ContentViewSelector : ContentView
    {
        public static readonly BindableProperty SelectedViewProperty =
            BindableProperty.Create(nameof(SelectedView), typeof(string), typeof(ContentViewSelector), propertyChanged: OnSelectedViewChanged);

        public string SelectedView
        {
            get => (string)GetValue(SelectedViewProperty);
            set => SetValue(SelectedViewProperty, value);
        }

        private static void OnSelectedViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ContentViewSelector)bindable;
            control.UpdateView((string)newValue);
        }

        private void UpdateView(string viewName)
        {
            switch (viewName)
            {
                case "View1":
                    Content = new Label { Text = "To jest widok 1", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                    break;
                case "View2":
                    Content = new Label { Text = "To jest widok 2", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                    break;
                case "View3":
                    Content = new Label { Text = "To jest widok 3", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                    break;
                case "View4":
                    Content = new Label { Text = "To jest widok 4", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                    break;
                default:
                    Content = new Label { Text = "Wybierz widok", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                    break;
            }
        }
    }
}
