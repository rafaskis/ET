// App.xaml.cs
using ET.Views;

namespace ET
{
    public partial class App : Application // Changed base class
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
