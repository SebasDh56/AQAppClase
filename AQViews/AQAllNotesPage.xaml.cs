namespace AQAppClase.AQViews;

public partial class AQAllNotesPage : ContentPage
{
	public AQAllNotesPage()
	{

        InitializeComponent();

        BindingContext = new AQModels.AQAllNotes();
    }

    protected override void OnAppearing()
    {
        ((AQModels.AQAllNotes)BindingContext).LoadNotes();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AQNotePage));
    }

    private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Get the note model
            var note = (AQModels.AQNote)e.CurrentSelection[0];

            // Should navigate to "NotePage?ItemId=path\on\device\XYZ.notes.txt"
            await Shell.Current.GoToAsync($"{nameof(AQNotePage)}?{nameof(AQNotePage.ItemId)}={note.Filename}");

            // Unselect the UI
            notesCollection.SelectedItem = null;
        }
    }
}