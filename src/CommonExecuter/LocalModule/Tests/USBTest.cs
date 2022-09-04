using eckumoc;

namespace tools.Hosted.Utils
{
    public class USBTest : TestingElement
    {
        protected override void OnTest()
        {
            var usb = new USBManager();
            usb.Init();
        }
    }
}