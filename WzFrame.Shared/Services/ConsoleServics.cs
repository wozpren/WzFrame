using Masuit.Tools.Core.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Services
{
    public class ConsoleServics
    {
        private StringWriter consoleOutput = new StringWriter();

        public ConsoleServics()
        {
            Console.SetOut(consoleOutput);
        }


        public string Get(int size = 10000, int offset = 0)
        {
            var sb = consoleOutput.GetStringBuilder();
            if (sb.Length < size)
                return sb.ToString();
            else
                return sb.ToString(offset, size);
        }


    }
}
