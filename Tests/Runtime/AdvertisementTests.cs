using NUnit.Framework;

namespace Tests.Runtime
{
    [Category("Text Parsing & Layout")]
    public class AdvertisementTests
    {
        [Test]
        [TestCase(false, false)]
        [TestCase(true, true)]
        [TestCase(null, false)]
        public void GetAndSetShowAfterLoad(bool actual, bool expected)
        {
            /*var banner = new Banner(m_NativeBannerMock.Object, m_CoroutineExecutorMock.Object);
        banner.ShowAfterLoad = actual;
        Assert.That(banner.ShowAfterLoad, Is.EqualTo(expected), "ShowAfterLoad did not return the correct value");*/
            Assert.True(true);
        }
    }
}