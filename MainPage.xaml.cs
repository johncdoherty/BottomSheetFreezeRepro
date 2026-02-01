using Syncfusion.Maui.Toolkit.BottomSheet;

namespace BottomSheetFreezeRepro;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.MainViewModel();
	}

	private void OnOpenBottomSheetClicked(object sender, EventArgs e)
	{
		ReproBottomSheet.IsOpen = true;
		ReproBottomSheet.State = BottomSheetState.HalfExpanded;
	}
}
