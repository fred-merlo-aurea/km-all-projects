using System.Diagnostics.CodeAnalysis;
using AutoFixture.Kernel;

namespace AUT.ConfigureTestProjects
{
    [ExcludeFromCodeCoverage]
    public class PropertiesPostProcessor : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder _builder;

        public PropertiesPostProcessor(ISpecimenBuilder builder)
        {
            this._builder = builder;
        }

        public object Create(object request, ISpecimenContext context)
        {
            dynamic specimen = this._builder.Create(request, context);
            if (specimen is NoSpecimen)
            {
                return specimen;
            }

            specimen.SetupAllProperties();
            return specimen;
        }
    }
}