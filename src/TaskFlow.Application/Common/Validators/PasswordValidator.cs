using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace TaskFlow.Application.Common.Validators;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "PasswordValidator";

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        if (password.Length < 8) return false;

        if (!UpperCaseLetter().IsMatch(password)) return false;

        if (!LowerCaseLetter().IsMatch(password)) return false;

        if (!Numbers().IsMatch(password)) return false;

        if (!SpecialSymbols().IsMatch(password)) return false;

        return true;
    }

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();

    [GeneratedRegex(@"[a-z+]")]
    private static partial Regex LowerCaseLetter();

    [GeneratedRegex(@"[0-9]")]
    private static partial Regex Numbers();

    [GeneratedRegex(@"[!@#$%^&*()_\-+=\[\]{};:'"",.<>?/\\|`~]")]
    private static partial Regex SpecialSymbols();

}