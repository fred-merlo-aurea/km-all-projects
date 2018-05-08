using System;

namespace KM.Common
{
    public static class Guard
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <see cref="arg"/> is null.
        /// </summary>
        /// <param name="arg">The argument to check for null</param>
        /// <param name="argName">The name of the argument to be used when throwing <see cref="ArgumentNullException"/></param>
        /// <param name="message">The message to be used when throwing <see cref="ArgumentNullException"/></param>
        public static void NotNull(object arg, string argName, string message = null)
        {
            For(() => arg == null, () => new ArgumentNullException(argName, message));
        }

        /// <summary>
        /// Throws the specified Exception if <see cref="arg"/> is null.
        /// </summary>
        /// <param name="arg">The argument to check for null</param>
        /// <param name="exception">The exception to be thrown if the argument is null</param>
        public static void NotNull(object arg, Func<Exception> exception)            
        {
            For(() => arg == null, exception);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <see cref="arg"/> is null or empty.
        /// </summary>
        /// <param name="arg">The argument to check for null or empty</param>
        /// <param name="argName">The name of the argument to be used when throwing <see cref="ArgumentException"/></param>
        /// <param name="message">The message to be used when throwing <see cref="ArgumentException"/></param>
        public static void NotNullOrEmpty(string arg, string argName, string message = null)
        {
            For(() => string.IsNullOrEmpty(arg), () => CreateArgumentException(message, argName));
        }

        /// <summary>
        /// Throws the specified Exception if <see cref="arg"/> is null or empty.
        /// </summary>
        /// <param name="arg">The argument to check for null or empty</param>
        /// <param name="exception">The exception to be thrown if the argument is null or empty</param>
        public static void NotNullOrEmpty(string arg, Exception exception)
        {
            For(() => string.IsNullOrEmpty(arg), () => exception);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <see cref="arg"/> is null or whitespace.
        /// </summary>
        /// <param name="arg">The argument to check for null or whitespace</param>
        /// <param name="argName">The name of the argument to be used when throwing <see cref="ArgumentException"/></param>
        /// <param name="message">The message to be used when throwing <see cref="ArgumentException"/></param>
        public static void NotNullOrWhitespace(string arg, string argName, string message = null)
        {
            For(() => string.IsNullOrWhiteSpace(arg), () => CreateArgumentException(message, argName));
        }

        /// <summary>
        /// Throws the specified Exception if <see cref="arg"/> is null or whitespace.
        /// </summary>
        /// <param name="arg">The argument to check for null or whitespace</param>
        /// <param name="exception">The exception to be thrown if the argument is null or whitespace</param>
        public static void NotNullOrWhitespace(string arg, Exception exception)
        {
            For(() => string.IsNullOrWhiteSpace(arg), () => exception);
        }

        /// <summary>
        /// Throws the specified Exception if <see cref="predicate"/> evaluates to True.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="predicate">The predicate condition that triggers the specified Exception to be thrown</param>
        /// <param name="exception">The exception to be thrown if <see cref="predicate"/> evaluates to True</param>
        public static void For<TException>(Func<bool> predicate, Func<TException> exception)
            where TException : Exception
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (predicate())
            {
                throw exception();
            }
        }

        /// <summary>
        /// Parses a string into an integer, throws InvalidOperationException in case TryParse failed.
        /// </summary>
        /// <param name="stringValue">A string that is expected to have a valid integer value</param>
        /// <returns>Interger value in case the provided string contains a valid interger, if not InvalidOperationException will be thrown</returns>
        public static int ParseStringToInt(string stringValue)
        {
            var result = 0;
            if (!int.TryParse(stringValue, out result))
            {
                throw new InvalidOperationException("Unable to parse string as integer");
            }

            return result;
        }

        private static ArgumentException CreateArgumentException(string message, string argName)
        {
            return new ArgumentException(message ?? argName, argName);
        }
    }
}
