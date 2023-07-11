namespace Shared.GameState;
[Serializable]
public class BackTrackGameState: AbstractGameState
{
    public int CorrectlyRecognizedNumbers;
    public int RequiredNumbersToWin;
    public int ShowNextFieldCounter;
    public List<int> PredictableNumbers;

    public BackTrackGameState(int correctlyRecognizedNumbers, int requiredNumbersToWin, int showNextFieldCounter, List<int> predictableNumbers)
    {
        CorrectlyRecognizedNumbers = correctlyRecognizedNumbers;
        RequiredNumbersToWin = requiredNumbersToWin;
        ShowNextFieldCounter = showNextFieldCounter;
        PredictableNumbers = predictableNumbers;
    }
}