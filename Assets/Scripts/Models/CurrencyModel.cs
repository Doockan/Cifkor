namespace Assets.Scripts.Models
{
  public class CurrencyModel
  {
    public int Amount { get; private set; }

    public void Add(int value) { Amount += value; }

    public bool TrySpend(int value)
    {
      if (Amount >= value)
      {
        Amount -= value;
        return true;
      }

      return false;
    }
  }

}