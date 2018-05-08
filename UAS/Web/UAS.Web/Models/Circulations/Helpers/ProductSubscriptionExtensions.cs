using static FrameworkUAS.BusinessLogic.Enums;
using FrameworkUAD_Lookup.Entity;
using KM.Common;

namespace UAS.Web.Models.Circulations.Helpers
{
    public static class ProductSubscriptionExtensions
    {
        public static bool IsMexico(this Country country)
        {
            Guard.NotNull(country, nameof(country));
            return country.ShortName.RemoveDash().SpaceInstedOfUnderscore() == CountriesWithRegions.MEXICO.ToString().SpaceInstedOfUnderscore();
        }

        public static bool IsCanada(this Country country)
        {
            Guard.NotNull(country, nameof(country));
            return country.ShortName == CountriesWithRegions.CANADA.ToString().SpaceInstedOfUnderscore();
        }

        public static bool IsUsa(this Country country)
        {
            Guard.NotNull(country, nameof(country));
            return country.ShortName == CountriesWithRegions.UNITED_STATES.ToString().SpaceInstedOfUnderscore();
        }

        public static string SpaceInstedOfUnderscore(this string inputString)
        {
            Guard.NotNull(inputString, nameof(inputString));
            return inputString.Replace("_", " ").Trim();
        }

        public static string RemoveDash(this string inputString)
        {
            Guard.NotNull(inputString, nameof(inputString));
            return inputString.Replace("-", "");
        }
    }
}