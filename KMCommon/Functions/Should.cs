namespace KM.Common.Functions
{
    public static class Should
    {
        public static bool NoneBeNull(params object[] values)
        {
            foreach (var value in values)
            {
                if (value == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AllBeNull(params object[] values)
        {
            foreach (var value in values)
            {
                if (value != null)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AnyBeNull(params object[] values)
        {
            foreach (var value in values)
            {
                if (value == null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AnyBeNonNull(params object[] values)
        {
            foreach (var value in values)
            {
                if (value != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
