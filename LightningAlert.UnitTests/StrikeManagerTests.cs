using LightningAlert.BAL;
using LightningAlert.DAL.Interfaces;
using LightningAlert.Models;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace LightningAlert.UnitTests
{
    public class StrikeManagerTests
    {
        private Mock<IDataProvider<Strike>> _dataProvider;
        private StrikeManager _target;

        [SetUp]
        public void Setup()
        {
            _dataProvider = new Mock<IDataProvider<Strike>>();
            _target = new StrikeManager(_dataProvider.Object);
        }

        [Test]
        public void TestConstructor()
        {
            Assert.NotNull(_target);
        }

        [Test]
        public void FilterStrike_WithValidData()
        {
            //Arrange
            var strike = new Strike { FlashType = FlashType.CloudToCloud};

            //Act
            _target.FilterStrike(strike);
        }

        [Test]
        public void FilterStrike_ThrowException()
        {
            //Arrange
            var strike = new Strike { FlashType = FlashType.HeartBeat };

            //Act

            //Assert
            Assert.Throws<Exception>(() => _target.FilterStrike(strike));
        }

        [Test]
        public async Task GetStrikesAsync_WithValidData()
        {
            //Arrange
            var strikeFileContent = "{\"flashType\":1,\"strikeTime\":1446760902510,\"latitude\":8.7020156,\"longitude\":-12.2736188,\"peakAmps\":3034,\"reserved\":\"000\",\"icHeight\":11829,\"receivedTime\":1446760915181,\"numberOfSensors\":6,\"multiplicity\":1}" + Environment.NewLine +
                                    "{\"flashType\":1,\"strikeTime\":1446760902380,\"latitude\":10.5799716,\"longitude\":-14.0589797,\"peakAmps\":3117,\"reserved\":\"000\",\"icHeight\":18831,\"receivedTime\":1446760915182,\"numberOfSensors\":8,\"multiplicity\":1}" + Environment.NewLine +
                                    "{\"flashType\":1,\"strikeTime\":1446760902523,\"latitude\":8.6972308,\"longitude\":-12.2895479,\"peakAmps\":3501,\"reserved\":\"000\",\"icHeight\":15392,\"receivedTime\":1446760915182,\"numberOfSensors\":7,\"multiplicity\":4}" + Environment.NewLine +
                                    "{\"flashType\":1,\"strikeTime\":1446760902526,\"latitude\":8.6764402,\"longitude\":-12.2806221,\"peakAmps\":4749,\"reserved\":\"000\",\"icHeight\":10276,\"receivedTime\":1446760915182,\"numberOfSensors\":7,\"multiplicity\":4}" + Environment.NewLine +
                                    "{\"flashType\":1,\"strikeTime\":1446760902546,\"latitude\":8.7053152,\"longitude\":-12.2771736,\"peakAmps\":3384,\"reserved\":\"000\",\"icHeight\":14477,\"receivedTime\":1446760915182,\"numberOfSensors\":6,\"multiplicity\":4}";

            var fakeFileBytes = Encoding.UTF8.GetBytes(strikeFileContent);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            _dataProvider.Setup(d => d.GetStream()).Returns(() => new StreamReader(fakeMemoryStream));

            //Act
            var strikes = _target.GetStrikesAsync();

            //Assert
            Assert.IsNotNull(strikes);
            Assert.AreEqual(5, (await strikes.ToListAsync()).Count);
        }

        [Test]
        public void GetStrikesAsync_ThrowJsonException()
        {
            //Arrange
            var strikeFileContent = "{\"flashType\":WRONGDATA,\"strikeTime\":1446760902510,\"latitude\":8.7020156,\"longitude\":-12.2736188,\"peakAmps\":3034,\"reserved\":\"000\",\"icHeight\":11829,\"receivedTime\":1446760915181,\"numberOfSensors\":6,\"multiplicity\":1}";

            var fakeFileBytes = Encoding.UTF8.GetBytes(strikeFileContent);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            _dataProvider.Setup(d => d.GetStream()).Returns(() => new StreamReader(fakeMemoryStream));

            //Act
            var strikes = _target.GetStrikesAsync();

            //Assert
            Assert.IsNotNull(strikes);
            Assert.Throws<JsonException>(() => { var result = strikes.ToListAsync().Result; });
        }
    }
}