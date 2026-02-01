using Syncfusion.Maui.Toolkit.BottomSheet;
using BottomSheetFreezeRepro.ViewModels;

namespace BottomSheetFreezeRepro;

public partial class MainPage : ContentPage
{
	private MainViewModel? _viewModel;

	public MainPage()
	{
		InitializeComponent();
		_viewModel = new MainViewModel();
		BindingContext = _viewModel;
	}

	private void OnOpenBottomSheetClicked(object sender, EventArgs e)
	{
		if (_viewModel?.UsePureMaui == true)
		{
			MauiBottomSheet.Show();
		}
		else
		{
			ReproBottomSheet.IsOpen = true;
			ReproBottomSheet.State = BottomSheetState.HalfExpanded;
		}
	}
}
