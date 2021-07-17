using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace VirtualGrid
{
    /// <summary>
    /// An abstract color object representation.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial struct Color
    {
        /// <summary>
        /// Gets the blue component of the color as a byte.
        /// </summary>
        public byte B => (byte)((Value >> 16) & 0xFF);

        /// <summary>
        /// Gets the green component of the color as a byte.
        /// </summary>
        public byte G => (byte)((Value >> 8) & 0xFF);

        /// <summary>
        /// Gets the red component of the color as a byte.
        /// </summary>
        public byte R => (byte)(Value & 0xFF);

        /// <summary>
        /// Gets the unsigned integer representing
        /// the color. On the form <c>0xKKBBGGRR</c>.
        /// </summary>
        public readonly uint Value;

        /// <summary>
        /// Constructor for Color with uint value.
        /// </summary>
        /// <param name="value"></param>
        public Color(uint value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor for Color with R,G, and B as byte.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(byte red, byte green, byte blue) : this(red + ((uint)green << 8) + ((uint)blue << 16))
        {
        }

        /// <summary>
        /// Constructor for Color with R,G, and B as float.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(float red, float green, float blue) : this((byte)(red * 255), (byte)(green * 255), (byte)(blue * 255))
        {
        }

        /// <summary>
        /// Constructor for Color with R,G, and B as double.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(double red, double green, double blue) : this((byte)(red * 255), (byte)(green * 255), (byte)(blue * 255))
        {
        }

        /// <summary>
        /// Create color instance from uint value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color FromRgb(uint value)
        {
            return new Color(((value & 0xFF0000) >> 16) | (value & 0xFF00) | ((value & 0xFF) << 16));
        }

        /// <inheritdoc/>
        public override string ToString() => $"{R}, {G}, {B} (0x{Value:X7})";
    }
}
