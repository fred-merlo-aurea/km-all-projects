using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects.Extensions;

namespace AUT.ConfigureTestProjects.Analyzer
{
    [ExcludeFromCodeCoverage]
    public static class CreateAnalyzer
    {

        public static bool IsAnyConstructorsPublic(Type type)
        {
            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                if (constructor.IsPublic)
                {
                    return true;
                }
            }

            return false;
        }

        public static ConstructorInfo GetDefaultPublicConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                if (constructor.IsPublic)
                {
                    return constructor;
                }
            }

            return null;
        }

        public static T CreateOrReturnStaticInstance<T>(IFixture fixture, out Exception exception, dynamic[] parameters = null) where T : class
        {
            var type = typeof(T);
            exception = null;
            var instance = type.Create<T>(out exception, parameters);

            if (instance != null)
            {
                return instance;
            }

            var constructor = GetDefaultPublicConstructor(type);

            if (constructor != null)
            {
                try
                {
                    instance = constructor.Invoke(parameters) as T;

                    if (instance != null)
                    {
                        return instance;
                    }
                }
                catch (Exception e)
                {
                    exception = e;

                    try
                    {
                        instance = fixture.Create<T>();
                        return instance;
                    }
                    catch (Exception ex2)
                    {
                        exception = ex2;
                    }
                }
            }

            return null;
        }

        public static bool DoesThrow<T>() where T : new()
        {
            var exception = GetThrownExceptionWhenCreate<T>();
            return exception != null;
        }

        public static bool DoesThrow<T>(dynamic[] parameters) where T : class, new()
        {
            var exception = GetThrownExceptionWhenCreate<T>(parameters);
            return exception != null;
        }

        public static bool DoesThrowWhenCreatedWithAutoFixture<T>(IFixture autoFixture, dynamic[] parameters) where T : class, new()
        {
            try
            {
                var instance = autoFixture.Create<T>();
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }

        public static Exception GetThrownExceptionWhenCreate<T>(dynamic[] parameters) where T : class, new()
        {
            try
            {
                var type = typeof(T);
                var instance = Activator.CreateInstance(type, parameters) as T;
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public static Exception GetThrownExceptionWhenCreate<T>() where T : new()
        {
            try
            {
                var instance = new T();
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
