using System;

namespace APP.Common
{
    public static class Mandate
    {
        public static void ParameterNotNull<T>(T value, string name) where T : class
        {
            That(value != null, () => new ArgumentNullException(name));
        }

        public static void That<TException>(bool condition, Func<TException> thrownException)
            where TException : System.Exception, new()
        {
            if (condition == false)
                throw thrownException();
        }
    }
}
