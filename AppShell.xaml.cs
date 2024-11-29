namespace AQAppClase
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AQViews.AQNotePage), typeof(AQViews.AQNotePage));
        }
    }
}
