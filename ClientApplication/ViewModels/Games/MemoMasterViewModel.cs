using ClientApplication.Utils;
using Shared;
using ClientApplication.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Shared.GameState;

namespace ClientApplication.ViewModels.Games;

public class MemoMasterViewModel: AbstractGameViewModel<MemoMasterGameState>
{
    private ObservableCollection<Card> _cards;
    public MemoMasterViewModel(INavigationService navigationService) : base(navigationService,
        GameType.MemoMaster)
    {
        Cards = new ObservableCollection<Card>();
        AddCards();
        GenerateSymbolID();
    }

    public override void StartGame(TaskDifficulty taskDifficulty, MemoMasterGameState? state)
    {
        if (taskDifficulty == TaskDifficulty.Hard)
        {
            
        }
        IsGameRunning = true;
    }

    public override MemoMasterGameState GetGameState()
    {
        return new MemoMasterGameState();
    }

    public override void StopGame()
    {
        IsGameRunning = false;
    }

    public int ImageChange
    {
        get { return ImageChange; }
        set
        {
            ImageChange = value;
            OnPropertyChanged(nameof(ImageChange));
        }
    }

    public ICommand ChangeCardCommand { get; }

    private void AddCards()
    {
        // Add cards with positions
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Cards.Add(new Card { Id= i*4+j, symbolID = 0, X = (j * 20), Y = (i * 20) });
            }
        }
    }
 
    private void GenerateSymbolID()
    {

        //HERE GENERATE RANDOM PAIRS OF SZMBOL ID 1-8
        foreach (Card card in Cards)
        {
            card.symbolID = 2;
        }
        Cards.ElementAt(0).symbolID = 1;
        Cards.ElementAt(4).symbolID = 1;
        Cards.ElementAt(6).symbolID = 1;

    }
    public ObservableCollection<Card> Cards
    {
        get => _cards;
        set
        {
            if (_cards != value)
            {
                _cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }
    }
}