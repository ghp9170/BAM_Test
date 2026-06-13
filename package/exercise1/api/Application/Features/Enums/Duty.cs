using StargateAPI.Application.Features.Enums;

namespace StargateAPI.Application.Features.Enums
{
    public enum Duty
    {

        Retired = 0
    }
public static class DutyExtensions
{
    public static string ToString(this Duty duty)
    {
        return duty switch
        {
            Duty.Retired => "RETIRED",
            _ => throw new ArgumentOutOfRangeException(nameof(duty), duty, null)
        };
    }
}
}
