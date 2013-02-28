namespace Glimpse.Package
{
    public class Settings : ISettings
    {
        private ISystemLogger _logger;

        public Settings()
        {
            LoggerProvider = new SystemLoggerProviderNull(); 
            Options = new SettingsExtensionOptions();

            LoadSettings();
        }

        public ISystemLoggerProvider LoggerProvider { get; private set; }

        public bool Debug { get; set; }

        public bool LoggingEnabled { get; set; }

        public bool LogEverything { get; set; }

        public string LoggingPath { get; set; }

        public bool ServiceEnabled { get; set; }

        public int MinServiceTriggerInterval { get; set; }

        public bool DisableAutoBuild { get; set; }

        public SettingsExtensionOptions Options { get; set; }

        public IRefreshReleaseRepositoryService RefreshReleaseRepositoryService { get; private set; }

        public IReleaseQueryService ReleaseQueryService { get; private set; }

        public IRefreshReleaseService RefreshReleaseService { get; private set; }

        public IReleaseService ReleaseService { get; private set; }

        public IReleaseQueryProvider QueryProvider { get; private set; }

        public void Initialize()
        {
            //Need to setup the logger first
            if (LoggingEnabled)
                LoggerProvider = Options.LoggerProvider ?? new SystemLoggerProviderLog4Net(this);

            _logger = LoggerProvider.CreateLogger(typeof(Settings));

            //Setting up the rest of the system
            _logger.Info("Settings - Setup Started");
            _logger.Debug(() => string.Format("Settings - Settings Extension Options - {0}", Options));

            var sqlFactory = new SqlFactory();

            var feedProvider = new NugetReleaseFeedProvider();
            var persistencyProvider = new ReleasePersistencyProvider(sqlFactory);
 
            QueryProvider = new CacheReleaseQueryProvider();
            ReleaseQueryService = new ReleaseQueryService(QueryProvider); 
            RefreshReleaseRepositoryService = new RefreshReleaseRepositoryService(feedProvider, persistencyProvider);
            RefreshReleaseService = new RefreshReleaseService(this, RefreshReleaseRepositoryService, QueryProvider);
            ReleaseService = new ReleaseService(QueryProvider);

            _logger.Info("Settings - Setup Finished");

            //Run system setup
            _logger.Info("Settings - Pre Initialize");
             
            RefreshReleaseService.Execute(true);

            _logger.Info("Settings - Post Initialize");
        }

        private void LoadSettings()
        {
            var configProcessor = new ConfigProcessor(new ConfigProvider(LoggerProvider.CreateLogger(typeof(ConfigProvider))));
            configProcessor.Process(this);
        }
    }
}