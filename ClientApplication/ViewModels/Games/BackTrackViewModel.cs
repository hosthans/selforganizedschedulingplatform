using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using ClientApplication.Utils;
using Shared;
using Shared.GameState;

namespace ClientApplication.ViewModels.Games;

public class BackTrackViewModel: AbstractGameViewModel<BackTrackGameState>
{
    private string _nextNumber = "";
    public string NextNumber
    {
        get => _nextNumber;
        set
        {
            _nextNumber = value;
            OnPropertyChanged(nameof(NextNumber));
        }
    }
    private readonly Random _random = new();
    private DispatcherTimer? _gameTimer;
    private DispatcherTimer? _showNumberTimer;
    private int _timeLeft;
    private List<int> _predictableNumbers = new();
    public int TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = value;
            OnPropertyChanged(nameof(TimeLeft));
        }
    }
    private int _showNextNumberTimeLeft = TimeSpanToShowSlot;
    public int ShowNextNumberTimeLeft
    {
        get => _showNextNumberTimeLeft;
        set
        {
            _showNextNumberTimeLeft = value;
            OnPropertyChanged(nameof(ShowNextNumberTimeLeft));
        }
    }
    private int _showNextFieldCounter;
    private const int GameDurationSeconds = 60;
    private int _correctlyRecognizedNumbers;
    private int _requiredNumbersToWin = 5;
    private const int TimeSpanToShowSlot = 3;
    private const int NBacks = 2;
    //All numbers shown in given GameDuration for example 60 sec GameDuration, 3 sec for every slot: 10 numbers shown multiply by 3 = 30 sec
    private const int NumbersToShow = 10;
    private const int MaxNumberToShow = NBacks*NumbersToShow-2;
    private bool _isNumberInserted = true;
    
    public event EventHandler<bool>? TypingEnabledEventHandler;
    public event EventHandler<int>? NumberInsertedEventHandler;
    public BackTrackViewModel(INavigationService navigationService) : base(navigationService, GameType.BackTrack)
    {
        NumberInsertedEvent();
    }

    public override void StartGame(TaskDifficulty taskDifficulty, BackTrackGameState? gameState)
    {
        if (taskDifficulty == TaskDifficulty.Hard)
        {
            _requiredNumbersToWin = 7;
        }
        TimeLeft = GameDurationSeconds;
        if (gameState != null)
        {
            SetGameState(gameState);
        }
        if (_showNextFieldCounter == 0)
        {
            ShowNextNumber();
        }else ShowNextNumberTimerTick(null, EventArgs.Empty);
        Logging.LogGameEvent("BackTrack started");
        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _gameTimer.Tick += GameTimerTick;
        _showNumberTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(TimeSpanToShowSlot)
        };
        _showNumberTimer.Tick += ShowNextNumberTimerTick;
        _showNumberTimer?.Start();
        _gameTimer?.Start();
        IsGameRunning = true;
    }

    public override void StopGame()
    {
        _showNextFieldCounter = 0;
        _correctlyRecognizedNumbers = 0;
        _isNumberInserted = true;
        _timeLeft = GameDurationSeconds;
        _gameTimer?.Stop();
        _showNumberTimer?.Stop();
        NextNumber = "";
        IsGameRunning = false;
    }

    public override BackTrackGameState GetGameState()
    {
        return new BackTrackGameState(_correctlyRecognizedNumbers, _requiredNumbersToWin, _showNextFieldCounter, _predictableNumbers);
    }

    private void GameTimerTick(object? sender, EventArgs e)
    {
        _timeLeft--;
        _showNextNumberTimeLeft--;
        ShowNextNumberTimeLeft = _showNextNumberTimeLeft;
        TimeLeft = _timeLeft;
        Logging.LogGameEvent($"BackTrack time left: {_timeLeft}, correctlyRecognizedNumbers: {_correctlyRecognizedNumbers}");
        CheckIfAlreadyWinOrLose();
    }

    private void ShowNextNumberTimerTick(object? sender, EventArgs e)
    {
        if (!_isNumberInserted)
        {
            //No number inserted event
            NumberInsertedEventHandler?.Invoke(null, -1);
        }
        if (_showNextFieldCounter % 2 == 1 && _showNextFieldCounter < MaxNumberToShow)
        {
            //Show next number
            ShowNextNumber();
        }
        else
        {
            //Enable inserting a number
            _showNextNumberTimeLeft = TimeSpanToShowSlot;
            _showNextFieldCounter++;
            NextNumber = "";
            _isNumberInserted = false;
            TypingEnabledEventHandler?.Invoke(null, true);
        }
    }

    private void ShowNextNumber()
    {
        NextNumber = "";
        _showNextNumberTimeLeft = TimeSpanToShowSlot;
        TypingEnabledEventHandler?.Invoke(null, false);
        var nextNumberAsInt = _random.Next(1, 10);
        NextNumber = nextNumberAsInt.ToString();
        _showNextFieldCounter++;
        _predictableNumbers.Add(nextNumberAsInt);
        Logging.LogGameEvent($"BackTrack show next Number: {nextNumberAsInt}");
    }

    private void CheckIfAlreadyWinOrLose()
    {
        bool? win = null;
        if (TimeLeft == 0)
        {
            win = false;
        }
        else if (TimeLeft == 0 && _correctlyRecognizedNumbers >= _requiredNumbersToWin)
        {
            win = true;
        }

        if (win != null)
        {
            RemoveActiveTask();
            MessageBox.Show((bool)win ? "Congratulations, you win!" : "Sorry, you lose.");
            Logging.LogGameEvent($"BackTrack {((bool)win ? "win" : "lose")}");
        }
    }

    private void NumberInsertedEvent()
    {
        if(NumberInsertedEventHandler == null){
            NumberInsertedEventHandler += (_, insertedNumber) =>
            { 
                _isNumberInserted = true;
                var predictableNumber = _predictableNumbers[^2];
                if (_showNextFieldCounter == NBacks * 2)
                {
                    predictableNumber = _predictableNumbers[^1];
                }
                if (insertedNumber == predictableNumber)
                {
                    Logging.LogGameEvent($"BackTrack insert correct number: {insertedNumber}");
                    _correctlyRecognizedNumbers++;
                }
                else
                {
                    Logging.LogGameEvent($"BackTrack insert false number: {insertedNumber}");
                }
            };
        }
    }

    private void SetGameState(BackTrackGameState gameState)
    {
        //To hold the time and the shown textFields constantly, the timeLeft is calculated with the shown textFields multiplied by TimeSpanToShowSlot
        _timeLeft = GameDurationSeconds - gameState.ShowNextFieldCounter * TimeSpanToShowSlot;
        TimeLeft = _timeLeft;
        _correctlyRecognizedNumbers = gameState.CorrectlyRecognizedNumbers;
        _requiredNumbersToWin = gameState.RequiredNumbersToWin;
        _showNextFieldCounter = gameState.ShowNextFieldCounter;
        _predictableNumbers = gameState.PredictableNumbers;
    }
    public void InvokeNumberInsertedEventHandler(int number)
    {
        NumberInsertedEventHandler?.Invoke(null, number);
    }
    
}