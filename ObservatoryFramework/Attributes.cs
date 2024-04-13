namespace Observatory.Framework;

#region Setting property attributes

/// <summary>
/// Specifies text to display as the name of the setting in the UI instead of the property name.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SettingDisplayName : Attribute
{
    private string name;

    /// <summary>
    /// Specifies text to display as the name of the setting in the UI instead of the property name.
    /// </summary>
    /// <param name="name">Name to display</param>
    public SettingDisplayName(string name)
    {
            this.name = name;
        }

    /// <summary>
    /// Accessor to get/set displayed name.
    /// </summary>
    public string DisplayName
    {
        get => name;
        set => name = value;
    }
}

/// <summary>
/// Indicates numeric properly should use a slider control instead of a numeric textbox with roller.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SettingNumericUseSlider : Attribute
{ }

/// <summary>
/// Specify bounds for numeric inputs.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SettingNumericBounds : Attribute
{
    private double minimum;
    private double maximum;
    private double increment;
    private int precision;

    /// <summary>
    /// Specify bounds for numeric inputs.
    /// </summary>
    /// <param name="minimum">Minimum allowed value.</param>
    /// <param name="maximum">Maximum allowed value.</param>
    /// <param name="increment">Increment between allowed values in slider/roller inputs.</param>
    /// <param name="precision">The number of digits to display for non integer values.</param>
    public SettingNumericBounds(double minimum, double maximum, double increment = 1.0, int precision = 1)
    {
            this.minimum = minimum;
            this.maximum = maximum;
            this.increment = increment;
            this.precision = precision;
        }

    /// <summary>
    /// Minimum allowed value.
    /// </summary>
    public double Minimum
    {
        get => minimum;
        set => minimum = value;
    }

    /// <summary>
    /// Maximum allowed value.
    /// </summary>
    public double Maximum
    {
        get => maximum;
        set => maximum = value;
    }

    /// <summary>
    /// Increment between allowed values in slider/roller inputs.
    /// </summary>
    public double Increment
    {
        get => increment;
        set => increment = value;
    }

    /// <summary>
    /// The number of digits to display for non integer values.
    /// </summary>
    public int Precision
    {
        get => precision;
        set => precision = value;
    }
}
#endregion