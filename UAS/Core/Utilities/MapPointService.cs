using MapPoint;

namespace Core_AMS.Utilities
{
    public class MapPointService
    {
        // Very important to not return the Interop type (MapPoint.Application) directly, 
        // because it cannot be mocked in unit test (neither through Moq nor MS Fakes)!
        public MapApplication CreateApplication()
        {
            var application = new Application();
            return new MapApplication(application);
        }
    }
}
