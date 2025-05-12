using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Console1
{
    internal class Test
    {
        private readonly IConfiguration _configuration;

        public Test(IConfiguration congfiguration)
        {
            this._configuration = congfiguration;
        }

        public void TestMethod()
        {
            // Access the configuration value
            var dataFromJsonFile = _configuration.GetSection("FileName").Value;
            Console.WriteLine(dataFromJsonFile);
        }
    }
}
