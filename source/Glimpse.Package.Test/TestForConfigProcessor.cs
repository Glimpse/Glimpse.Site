using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Moq;
using Xunit;

namespace Glimpse.Package.Test
{
    public class TestForConfigProcessor
    {
        public class UsingProcessor
        {
            [Fact]
            public void ShouldBeAbleToFullyLoadConfig()
            {
                var section = new ConfigSectionGlimpse(); 
                section.UseOfflineData = true;

                var logging = new ConfigElementLogging();
                section.Logging = logging;
                logging.Enabled = true;
                logging.LogEverything = true;
                logging.LoggingPath = "Test";

                var services = new ConfigElementServices();
                section.Services = services; 
                services.MinTriggerInterval = 10;
                 
                var configProvider = new Mock<IConfigProvider>();
                configProvider.Setup(x => x.GetSection<ConfigSectionGlimpse>("")).Returns(section);

                var settings = new TestSettings();
                settings.LoggerProvider = new SystemLoggerProviderNull(); 

                var configProcessor = new ConfigProcessor(configProvider.Object);
                configProcessor.Process(settings);

                Assert.True(settings.Debug);
                Assert.True(settings.UseOfflineData);
                Assert.True(settings.LoggingEnabled);
                Assert.True(settings.LogEverything);
                Assert.Equal("Test", settings.LoggingPath);
                Assert.Equal(10, settings.MinServiceTriggerInterval); 
            }

            [Fact]
            public void ShouldBeAbleToLoadFromProviders()
            {
                var section = new ConfigSectionGlimpse(); 

                var configProvider = new Mock<IConfigProvider>();
                configProvider.Setup(x => x.GetSection<ConfigSectionGlimpse>("")).Returns(section);

                var settings = new TestSettings();
                settings.LoggerProvider = new SystemLoggerProviderNull(); 

                var configProcessor = new ConfigProcessor(configProvider.Object);
                configProcessor.Process(settings);
            }
        }
 
        #region Support Class 
         
        public class TestSettings : ISettings
        { 
            public ISystemLoggerProvider LoggerProvider { get; set; } 
            public bool LoggingEnabled { get; set; }
            public bool LogEverything { get; set; }
            public string LoggingPath { get; set; } 
            public int MinServiceTriggerInterval { get; set; }
            public bool UseOfflineData { get; set; }
            public IRefreshReleaseRepositoryService RefreshReleaseRepositoryService { get; private set; }
            public IReleaseQueryService ReleaseQueryService { get; private set; }
            public IRefreshReleaseService RefreshReleaseService { get; private set; }
            public IReleaseService ReleaseService { get; private set; }
            public IReleaseQueryProvider QueryProvider { get; private set; }

            public void Initialize()
            {
                throw new NotImplementedException();
            } 
        }

        public class TestSystemLoggerProvider : ISystemLoggerProvider
        {
            public ISystemLogger CreateLogger()
            {
                throw new NotImplementedException();
            }

            public ISystemLogger CreateLogger(Type name)
            {
                throw new NotImplementedException();
            }

            public ISystemLogger CreateLogger(string name)
            {
                throw new NotImplementedException();
            }
        }

        public class TestConfigProvider : IConfigProvider
        {
            public string GetAppSetting(string name)
            {
                throw new NotImplementedException();
            }

            public DbProviderFactory GetDbProviderFactory(string connectionStringName)
            {
                throw new NotImplementedException();
            }

            public string GetConnectionString(string connectionStringName)
            {
                throw new NotImplementedException();
            }

            public T GetSection<T>(string name) where T : class
            {
                throw new NotImplementedException();
            }
        } 

        #endregion
    }
}
