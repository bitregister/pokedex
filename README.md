# Pokedex

## Running the application

Once the repository has been cloned, the solution can be run from within Visual Studio 2019 (latest build 16.10.0) using with the IIS, Pokedex.WebApi or in the Docker profile. It requires the .Net 5 SDK (which should come as part of Visual Studio)

This has been tested in Visual Studio on both a Windows and Mac so should be fine to run in a 'non-windows' environment.

## Design notes

Several patterns have been employed to help ensure SOLID principles are adhered to as much as possible, this includes: 
1. A repository pattern to encapsulate the calls the 3rd Party Apis (PokeApi and FunTranslation), this helps ensure that the responsibility of querying these data sources is kept within these classes and can be easily mocked.
2. A strategy pattern to assist with deciding what translation to apply, this is helped by a factory pattern to return which strategy to use. The idea behind this is we can easily add additional translations with the business logic to decide which translation to use only in the `TranslationService` 
3. By employing the above I've kept all business logic out of the web api controllers which are now very lightweight.
4. I've made significant use of the Flurlhttp library, an alternative that I considered was the native HttpClient but I like the fluent building of the Urls and this also provides a good way of testing the underlying HttpClient.

## Further implementation for running in Production

If this was going to be deployed to Production I would recommend the following additions.

1. Adding response caching and rate limiting, this could be provided by an API gateway.
2. Adding application monitoring, in Azure I would do this by making use of Application Insights.
3. Logs will need to be sent to some central source, in Azure I'd use Log Analytics (via Serilog sink)
4. The application will also be configured to run only over HTTPS (currently just HTTP).
5. If necessary, the config files could be modified to include specific settings for the various environment required.