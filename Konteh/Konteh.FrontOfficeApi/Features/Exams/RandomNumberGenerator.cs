namespace Konteh.FrontOfficeApi.Features.Exams;
public class RandomNumberGenerator : IRandomNumberGenerator
{
    private readonly Random _random = new Random();
    public int Next(int maxValue) => _random.Next(maxValue);
}
