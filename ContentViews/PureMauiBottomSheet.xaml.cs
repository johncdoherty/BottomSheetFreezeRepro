using Microsoft.Maui.Controls;

namespace BottomSheetFreezeRepro.ContentViews;

public partial class PureMauiBottomSheet : Grid
{
	private double _initialTranslationY;
	private double _totalTranslationY;

	public PureMauiBottomSheet()
	{
		InitializeComponent();
	}

	public void Show()
	{
		IsVisible = true;
		BottomSheetBorder.TranslationY = Height;
		BottomSheetBorder.TranslateToAsync(0, 0, 250, Easing.CubicOut);
	}

	public void Hide()
	{
		BottomSheetBorder.TranslateToAsync(0, Height, 250, Easing.CubicIn).ContinueWith(_ =>
		{
			MainThread.BeginInvokeOnMainThread(() => IsVisible = false);
		});
	}

	private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
	{
		switch (e.StatusType)
		{
			case GestureStatus.Started:
				_initialTranslationY = BottomSheetBorder.TranslationY;
				break;

			case GestureStatus.Running:
				// Only allow dragging down
				var newTranslationY = _initialTranslationY + e.TotalY;
				if (newTranslationY >= 0)
				{
					BottomSheetBorder.TranslationY = newTranslationY;
					_totalTranslationY = e.TotalY;
				}
				break;

			case GestureStatus.Completed:
			case GestureStatus.Canceled:
				// If dragged down more than 100 pixels, hide it
				if (_totalTranslationY > 100)
				{
					Hide();
				}
				else
				{
					// Snap back to position
					BottomSheetBorder.TranslateToAsync(0, 0, 150, Easing.SpringOut);
				}
				_totalTranslationY = 0;
				break;
		}
	}
}
