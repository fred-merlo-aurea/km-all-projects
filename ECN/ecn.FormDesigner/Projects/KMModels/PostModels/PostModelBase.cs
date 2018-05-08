using System;

namespace KMModels.PostModels
{
    public abstract class PostModelBase : ModelBase
    {
        private const string Prefix = "Partials/_";
        private const string Postfix = "PostModel";

        public string GetPartialViewName()
        {
            return Prefix + GetType().Name.Replace(Postfix, string.Empty);
        }
    }
}