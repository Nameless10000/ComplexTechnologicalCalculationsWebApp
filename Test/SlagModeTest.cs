using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    /// <summary>
    /// Тесты для либы SlagMode
    /// </summary>
    public class SlagModeTest
    {
        [Fact]
        public void getKey()
        {
            var _mathLib = new SlagMode("localhost:7258");

            var key = _mathLib.GetTokenFromServer(
                new UserAuthData 
                {
                    UserName = "Login",
                    Password = "Password"
                });

            Assert.Equal("тут должен быть ключ", key);
        }
    }
}
