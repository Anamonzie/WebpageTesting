[assembly: LevelOfParallelism(20)]
[assembly: Parallelizable(scope: ParallelScope.All)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]

namespace EpamWebTests
{
    [SetUpFixture]
    public sealed class InitTestRun
    {

    }
}
