using Domain.Constants;

namespace Domain.Others.Utils
{
    public static class EnumUtils
    {
        /// <summary>
        /// Convierte un string a un enum del tipo especificado.
        /// </summary>
        /// <typeparam name="T">El tipo de enum al que se desea convertir.</typeparam>
        /// <param name="value">El valor en formato string que se desea convertir.</param>
        /// <returns>El valor convertido al tipo de enum especificado.</returns>
        public static T ToEnum<T>(this string value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(DomainConstants.NOTNULL_OR_EMPTY, nameof(value));
            }
            if (Enum.TryParse<T>(value, true, out var result))
            {
                return result;
            }
            throw new ArgumentException($"El valor '{value}' no es un miembro válido del enum {typeof(T).Name}.");
        }
    }
}
