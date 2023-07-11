using System.Windows.Threading;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;
using Shared;

namespace TestProject1.GameTests;

[TestFixture]
public class BackTrackGameTest
{
    private readonly BackTrackViewModel _backTrackViewModel = new (NavigationService.GetInstance());
    [Test] 
    public async Task ShowTwoInitialNumbers()
    { 
         await WpfContext.Run(async() =>
        {
            _backTrackViewModel.StartGame(TaskDifficulty.Easy, null);
            // Act
            await Task.Delay(3000);
            
            // Assert
            Assert.That(_backTrackViewModel.GetGameState().ShowNextFieldCounter, Is.EqualTo(2));
        });
    }
}