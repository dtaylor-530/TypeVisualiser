namespace TypeVisualiser.Geometry
{
    public static class DoubleExtension
    {
        public static bool IsBetween(this double instance, double lower, double upper)
        {
            return (instance >= lower && instance <= upper);
        }
    }
}
