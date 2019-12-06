using AutoMapper;
using BitWaves.JudgeBoard.Models;
using NUnit.Framework;

namespace BitWaves.JudgeBoard.UnitTest
{
    /// <summary>
    /// Provide unit tests for <see cref="ModelMappingProfile"/> class.
    /// </summary>
    public sealed class ModelMappingTests
    {
        [Test]
        public void TestModeMapping()
        {
            var config = new MapperConfiguration(opt => opt.AddProfile<ModelMappingProfile>());

            Assert.DoesNotThrow(() => config.AssertConfigurationIsValid());
        }
    }
}
