namespace App.Model.Managers
{
    public interface IPositioningStrategy
    {
        Tile Left(Tile selected);
        Tile Right(Tile slected);
        Tile Up(Tile slected);
        Tile Down(Tile slected);
    }
}