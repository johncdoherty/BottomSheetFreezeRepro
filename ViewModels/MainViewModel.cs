using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BottomSheetFreezeRepro.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
	public ObservableCollection<string> Items { get; } =
	[
		"Item A",
		"Item B",
		"Item C",
		"Item D",
		"Item E"
	];

	private int _currentIndex;

	public int CurrentIndex
	{
		get => _currentIndex;
		set
		{
			if (_currentIndex == value)
			{
				return;
			}

			_currentIndex = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(CurrentItem));
			OnPropertyChanged(nameof(CanGoBack));
			OnPropertyChanged(nameof(CanGoForward));
		}
	}

	public string CurrentItem => Items.Count == 0 ? string.Empty : Items[CurrentIndex];

	public int ItemCount => Items.Count;

	public bool HasMultipleItems => Items.Count > 1;

	public bool CanGoBack => CurrentIndex > 0;

	public bool CanGoForward => CurrentIndex < Items.Count - 1;

	private bool _showWorkaround;

	public bool ShowWorkaround
	{
		get => _showWorkaround;
		set
		{
			if (_showWorkaround == value)
			{
				return;
			}

			_showWorkaround = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(ScenarioButtonText));
		}
	}

	public string ScenarioButtonText => ShowWorkaround ? "🟢 FIXED (tap to see broken)" : "🔴 BROKEN (tap to see fixed)";

	public ICommand NextCommand { get; }

	public ICommand PrevCommand { get; }

	public ICommand ToggleScenarioCommand { get; }

	public MainViewModel()
	{
		NextCommand = new Command(Next);
		PrevCommand = new Command(Previous);
		ToggleScenarioCommand = new Command(ToggleScenario);
	}

	private void Next()
	{
		if (Items.Count == 0)
		{
			return;
		}

		if (CurrentIndex < Items.Count - 1)
		{
			CurrentIndex++;
		}
	}

	private void Previous()
	{
		if (Items.Count == 0)
		{
			return;
		}

		if (CurrentIndex > 0)
		{
			CurrentIndex--;
		}
	}

	private void ToggleScenario()
	{
		ShowWorkaround = !ShowWorkaround;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
