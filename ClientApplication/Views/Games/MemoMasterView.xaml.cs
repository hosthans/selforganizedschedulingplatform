using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;

namespace ClientApplication.Views.Games;

public partial class MemoMasterView : UserControl
{

    private readonly MemoMasterViewModel memoMasterViewModel;
    public MemoMasterView()
    {
        InitializeComponent();
        memoMasterViewModel = new MemoMasterViewModel(NavigationService.GetInstance());
        DataContext = memoMasterViewModel;

        Loaded += ShowCard_Loaded;
    }

    private void ShowCard_Loaded(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this);
        if (parentWindow != null)
        {
            // PreviewKeyDown on parent window ensures that the event will always arrive here
            parentWindow.PreviewKeyDown += OnPreviewKeyDown;
        }
    }
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            //double newRectangleX = memoMasterViewModel.Cards - memoMasterViewModel.Cards();

            e.Handled = true;
        }
        else if (e.Key == Key.Right)
        {
            //double newRectangleX = memoMasterViewModel.RectangleX + memoMasterViewModel.GetRectangleSpeed();

            e.Handled = true;
        }
    }

    bool toggle = false;
    private void OnCardChanged(object sender, MouseButtonEventArgs e)
    {
        
    }
}