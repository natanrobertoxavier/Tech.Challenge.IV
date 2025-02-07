using TechTalk.SpecFlow;

namespace Tech.Challenge.Persistence.Functional.Tests.Shared.Base;

public class BaseStep(ScenarioContext scenarioContext)
{
    protected ScenarioContext _scenarioContext = scenarioContext;
}
