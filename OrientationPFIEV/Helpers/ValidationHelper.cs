using System.Windows.Forms;

namespace OrientationPFIEV.Helpers;

/// <summary>
/// Input validation helpers ported from VB6 moMain.bas (CheckAsciiEntier / CheckAsciiReel).
/// Designed to be wired to KeyPress events on TextBox controls.
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Returns true if <paramref name="keyChar"/> is acceptable in an integer-only field.
    /// Allows digits and backspace; rejects everything else.
    /// Port of VB6 CheckAsciiEntier.
    /// </summary>
    public static bool IsIntegerKey(char keyChar)
        => char.IsDigit(keyChar) || keyChar == '\b';

    /// <summary>
    /// Returns true if <paramref name="keyChar"/> is acceptable in a decimal number field.
    /// Allows digits, period, comma (French decimal separator), and backspace.
    /// Port of VB6 CheckAsciiReel.
    /// </summary>
    public static bool IsDecimalKey(char keyChar)
        => char.IsDigit(keyChar) || keyChar == '.' || keyChar == ',' || keyChar == '\b';

    /// <summary>
    /// Suppresses the key press event if the character is not valid for an integer field.
    /// Wire to TextBox.KeyPress: txt.KeyPress += ValidationHelper.EnforceInteger;
    /// </summary>
    public static void EnforceInteger(object? sender, KeyPressEventArgs e)
    {
        if (!IsIntegerKey(e.KeyChar))
            e.Handled = true;
    }

    /// <summary>
    /// Suppresses the key press event if the character is not valid for a decimal field.
    /// Wire to TextBox.KeyPress: txt.KeyPress += ValidationHelper.EnforceDecimal;
    /// </summary>
    public static void EnforceDecimal(object? sender, KeyPressEventArgs e)
    {
        if (!IsDecimalKey(e.KeyChar))
            e.Handled = true;
    }

    /// <summary>
    /// Returns true if <paramref name="value"/> is a non-empty, non-whitespace string.
    /// </summary>
    public static bool IsRequired(string? value)
        => !string.IsNullOrWhiteSpace(value);
}
