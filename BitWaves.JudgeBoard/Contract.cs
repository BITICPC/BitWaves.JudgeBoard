using System;

namespace BitWaves.JudgeBoard
{
    /// <summary>
    /// Provide pre-condition assertions.
    /// </summary>
    internal static class Contract
    {
        /// <summary>
        /// Ensure the given value is not null.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="variableName">The name of the variable at the call site.</param>
        /// <exception cref="ArgumentNullException"><see cref="value"/> is null.</exception>
        public static void NotNull(object value, string variableName)
        {
            if (value == null)
                throw new ArgumentNullException(variableName);
        }

        /// <summary>
        /// Ensure the given integer is positive.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="variableName">The name of the variable at the call site.</param>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="value"/> is zero or negative.</exception>
        public static void Positive(int value, string variableName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(variableName);
        }

        /// <summary>
        /// Ensure that the given integer is zero or positive.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="variableName">The name of the value at call site.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void NonNegative(int value, string variableName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(variableName);
        }

        /// <summary>
        /// Ensures that <paramref name="startIndex"/> and <see cref="length"/> properly describes a sub-region of the
        /// given array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The start index of the sub-region.</param>
        /// <param name="length">The length of the sub-region.</param>
        /// <param name="startIndexVariableName">
        /// The name of the <see cref="startIndex"/> variable at the call site.
        /// </param>
        /// <param name="lengthVariableName">The name of the <see cref="length"/> variable at the call site.</param>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="startIndex"/> is negative
        ///     or
        ///     <paramref name="startIndex"/> is out of range
        ///     or
        ///     <paramref name="length"/> is negative
        ///     or
        ///     <paramref name="startIndex"/> + <paramref name="length"/> is out of range.
        /// </exception>
        public static void RegionOf<T>(T[] array, int startIndex, int length,
                                       string startIndexVariableName,
                                       string lengthVariableName)
        {
            if (startIndex < 0 || startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(startIndexVariableName);
            if (length < 0)
                throw new ArgumentOutOfRangeException(lengthVariableName);
            if (startIndex + length > array.Length)
                throw new ArgumentOutOfRangeException($"{startIndexVariableName},{lengthVariableName}");
        }
    }
}
