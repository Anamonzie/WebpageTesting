[assembly: LevelOfParallelism(10)]
[assembly: Parallelizable(scope: ParallelScope.All)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]

namespace EpamWebTests
{
    [SetUpFixture]
    public sealed class InitTestRun
    {

    }
}
