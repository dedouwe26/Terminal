namespace OxDED.Terminal;


/// <summary>
/// These are TERMINAL-DEFINED colors.
/// </summary>
public enum Colors : byte {
    /// <summary>
    /// 
    /// </summary>
    Black = 30,
    /// <summary>
    /// 
    /// </summary>
    Red = 31,
    /// <summary>
    /// 
    /// </summary>
    Green = 32,
    /// <summary>
    /// 
    /// </summary>
    Yellow = 33,
    /// <summary>
    /// 
    /// </summary>
    Blue = 34,
    /// <summary>
    /// 
    /// </summary>
    Magenta = 35,
    /// <summary>
    /// 
    /// </summary>
    Cyan = 36,
    /// <summary>
    /// 
    /// </summary>
    White = 37,
    
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightBlack = 90,
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightRed = 91,
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightGreen = 92,
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightYellow = 93,
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightBlue = 94,
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightMagenta = 95,
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightCyan = 96,
    /// <summary>
    /// Only in terminals that support the aixterm specification.
    /// </summary>
    BrightWhite = 97,

    /// <summary>
    /// Resets color.
    /// </summary>
    Default = 39,
}

/// <summary>
/// Represents a color for a terminal.
/// </summary>
public class Color {
    /// <summary>
    /// The 8-bit (generated) table color.
    /// </summary>
    public byte? tableColor = null;
    /// <summary>
    /// The basic color set.
    /// </summary>
    public Colors? paletteColor = null;
    /// <summary>
    /// 24-bit True color.
    /// </summary>
    public (byte r, byte g, byte b)? trueColor = null;
    /// <summary>
    /// If it is a true color.
    /// </summary>
    public bool HasTrueColor {get { return (!(tableColor.HasValue && paletteColor.HasValue))&&trueColor.HasValue; }}
    /// <summary>
    /// If it is a palette color.
    /// </summary>
    public bool HasPaletteColor {get { return (!(tableColor.HasValue && trueColor.HasValue))&&paletteColor.HasValue; }}
    /// <summary>
    /// If it is a table color.
    /// </summary>
    public bool HasTableColor {get { return (!(paletteColor.HasValue && trueColor.HasValue))&&tableColor.HasValue; }}
    
    /// <summary>
    /// Creates a terminal color from a basic set of 8 terminal-defined colors.
    /// </summary>
    /// <param name="color">The color from the basic set.</param>
    public Color(Colors color) {
        paletteColor = color;
    }
    /// <summary>
    /// Creates a terminal color from a color table. <para/>
    /// See ANSI documentation on 256 (8-bit) color codes.
    /// </summary>
    /// <param name="color">The position in the color table.</param>
    public Color(byte color) {
        tableColor = color;
    }
    /// <summary>
    /// Creates a terminal color from rgb.
    /// </summary>
    /// <param name="r">The r component.</param>
    /// <param name="g">The g component.</param>
    /// <param name="b">The b component.</param>
    public Color(byte r, byte g, byte b) {
        trueColor = (r, g, b);
    }
    /// <summary>
    /// Creates a terminal color from a hex code
    /// </summary>
    /// <param name="hex">The hex code without the (#), must be 6 or longer.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public Color(string hex) {
        if (hex.Length < 6) { throw new ArgumentOutOfRangeException(nameof(hex), "The length was too small."); }
        trueColor = (Convert.ToByte(hex[..2], 16), Convert.ToByte(hex.Substring(2, 2), 16), Convert.ToByte(hex.Substring(4, 2), 16));
    }
    /// <summary>
    /// Creates an ANSI string for the background color.
    /// </summary>
    /// <returns>The ANSI string.</returns>
    /// <exception cref="Exception"></exception>
    public string ToBackgroundANSI() {
        if (HasPaletteColor&&paletteColor.HasValue) {
            return ANSI.BasicSetBackgroundColor((byte)paletteColor.Value);
        }
        if (HasTrueColor&&trueColor.HasValue) {
            return ANSI.BackgroundTrueColor(trueColor.Value.r, trueColor.Value.g, trueColor.Value.b);
        }
        if (HasTableColor&&tableColor.HasValue) {
            return ANSI.TableBackgroundColor(tableColor.Value);
        }
        throw new Exception("No possible color.");
    }
    /// <summary>
    /// Creates an ANSI string for the foreground color.
    /// </summary>
    /// <returns>The ANSI string.</returns>
    /// <exception cref="Exception"></exception>
    public string ToForegroundANSI() {
        if (HasPaletteColor&&paletteColor.HasValue) {
            return ANSI.BasicSetForegroundColor((byte)paletteColor.Value);
        }
        if (HasTrueColor&&trueColor.HasValue) {
            return ANSI.ForegroundTrueColor(trueColor.Value.r, trueColor.Value.g, trueColor.Value.b);
        }
        if (HasTableColor&&tableColor.HasValue) {
            return ANSI.TableForegroundColor(tableColor.Value);
        }
        throw new Exception("No possible color.");
    }
    /// <summary>
    /// 255, 0, 0
    /// </summary>
    public static readonly Color Red = new(255, 0, 0);
    /// <summary>
    /// 0, 255, 0
    /// </summary>
    public static readonly Color Green = new(0, 255, 0);
    /// <summary>
    /// 0, 0, 255
    /// </summary>
    public static readonly Color Blue = new(0, 0, 255);
    /// <summary>
    /// 255, 80, 80
    /// </summary>
    public static readonly Color LightRed = new(255, 80, 80);
    /// <summary>
    /// 30, 190, 30
    /// </summary>
    public static readonly Color DarkGreen = new(30, 190, 30);
    /// <summary>
    /// 30, 30, 190
    /// </summary>
    public static readonly Color DarkBlue = new(30, 30, 190);
    /// <summary>
    /// 255, 255, 0
    /// </summary>
    public static readonly Color Yellow = new(255, 255, 0);
    /// <summary>
    /// 255, 0, 255
    /// </summary>
    public static readonly Color Magenta = new(255, 0, 255);
    /// <summary>
    /// 0, 255, 255
    /// </summary>
    public static readonly Color Cyan = new(0, 255, 255);
    /// <summary>
    /// 255, 160, 0
    /// </summary>
    public static readonly Color Orange = new(255, 160, 0);
    /// <summary>
    /// 180, 180, 180
    /// </summary>
    public static readonly Color Gray = new(180, 180, 180);
    /// <summary>
    /// 64, 64, 64
    /// </summary>
    public static readonly Color DarkGray = new(64, 64, 64);
    /// <summary>
    /// 0, 0, 0
    /// </summary>
    public static readonly Color Black = new(0, 0, 0);
    /// <summary>
    /// 255, 255, 255
    /// </summary>
    public static readonly Color White = new(255, 255, 255);
    /// <summary>
    /// 
    /// </summary>
    public static implicit operator Color(Colors color) { return new(color); }
    /// <summary>
    /// 
    /// </summary>
    public static explicit operator Colors?(Color color) {  return color.paletteColor; }
    /// <summary>
    /// 
    /// </summary>
    public static explicit operator Colors(Color color) {
        if (color.HasPaletteColor&&color.paletteColor.HasValue) { return color.paletteColor.Value; }
        throw new Exception("No palette color.");
    }
}