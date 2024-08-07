namespace MAUIToolkit.Sample
{
    public partial class MainPage : ContentPage
    {
        private bool hasSignature = false;
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            SignaturePad.Clear();
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            Image.Source = SignaturePad.ToImageSource();
        }

        private void OnDrawStarted(object sender, System.ComponentModel.CancelEventArgs e)
        {
            hasSignature = true;
        }

        private void DrawCompleted(object sender, EventArgs e)
        {
            Image.Source = SignaturePad.ToImageSource();
        }
    }

}
