

using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;
using Microsoft.Extensions.Options;

namespace Test
{
    /// <summary>
    /// Тесты для либы SlagMode
    /// </summary>
    public class SlagModeTest (IOptions<ExternalServerDomain> serverAddress)
    {
        [Fact]
        public void getKey()
        {
            var _mathLib = new SlagMode(serverAddress);

            var key = _mathLib.GetTokenFromServer(
                new UserAuthData 
                {
                    UserName = "Login",
                    Password = "Password"
                });
            var res = key.Split('.');
            Assert.Equal(3, res.Length);
        }
    }
}
