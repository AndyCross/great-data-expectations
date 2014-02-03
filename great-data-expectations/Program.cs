using System;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations.Generation;

namespace GreatExpectations
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var expectationGenerator = new ExpectationGenerator();
            var expectations = expectationGenerator.GenerateExpectations(DateTime.Now.AddHours(-24D), DateTime.Now);

            Assert.Expectations(expectations);

            Console.ReadLine();
        }

    }
}
