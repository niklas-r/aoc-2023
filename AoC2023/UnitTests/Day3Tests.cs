using Day3;

namespace UnitTests;

// I can't find the bug for this day, so I need to write tests to find it
public class Day3Tests
{
    [TestCaseSource(nameof(PartOneTestCases))]
    public void PartOneTests(IEnumerable<string> input, int expected)
    {
        var p1 = Day3Impl.P1(input.ToList());
        Assert.That(p1, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(PartTwoTestCases))]
    public void PartTwoTests(IEnumerable<string> input, int expected)
    {
        var p2 = Day3Impl.P2(input.ToList());
        Assert.That(p2, Is.EqualTo(expected));
    }

    private static IEnumerable<TestCaseData> PartOneTestCases
    {
        get
        {
            yield return new TestCaseData(new List<string>
            {
                "...123...",
                "..*......"
            }, 123);
            yield return new TestCaseData(new List<string>
            {
                "...234...",
                "..-......"
            }, 234);
            yield return new TestCaseData(new List<string>
            {
                "...345...",
                "../......"
            }, 345);
            yield return new TestCaseData(new List<string>
            {
                "...456...",
                "..=......"
            }, 456);
            yield return new TestCaseData(new List<string>
            {
                "...567...",
                "..%......"
            }, 567);
            yield return new TestCaseData(new List<string>
            {
                "...678...",
                "..@......"
            }, 678);
            yield return new TestCaseData(new List<string>
            {
                "...789...",
                "..&......"
            }, 789);
            yield return new TestCaseData(new List<string>
            {
                "...10....",
                "..$......"
            }, 10);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                "...700...",
                "........."
            }, 0);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                "20-......",
                "........."
            }, 20);
            yield return new TestCaseData(new List<string>
            {
                "..-......",
                "30.......",
                "........."
            }, 30);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                "40.......",
                "..-......"
            }, 40);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                "50.......",
                "-........"
            }, 50);
            yield return new TestCaseData(new List<string>
            {
                "-........",
                "60.......",
                "........."
            }, 60);
            yield return new TestCaseData(new List<string>
            {
                ".-.......",
                "70.......",
                "........."
            }, 70);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                "80.......",
                ".-......."
            }, 80);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                ".......90",
                ".-......."
            }, 0);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                "......100",
                "........-"
            }, 100);
            yield return new TestCaseData(new List<string>
            {
                ".........",
                "......110",
                ".......-."
            }, 110);

            yield return new TestCaseData(new List<string>
            {
                ".........",
                "......120",
                "......-.."
            }, 120);

            yield return new TestCaseData(new List<string>
            {
                ".........",
                "......130",
                ".....-..."
            }, 130);

            yield return new TestCaseData(new List<string>
            {
                ".........",
                ".....-140",
                "........."
            }, 140);

            yield return new TestCaseData(new List<string>
            {
                ".....-...",
                "......150",
                "........."
            }, 150);

            yield return new TestCaseData(new List<string>
            {
                "......-..",
                "......160",
                "........."
            }, 160);

            yield return new TestCaseData(new List<string>
            {
                ".......-.",
                "......170",
                "........."
            }, 170);

            yield return new TestCaseData(new List<string>
            {
                "........-",
                "......180",
                "........."
            }, 180);
        }
    }

    private static IEnumerable<TestCaseData> PartTwoTestCases
    {
        get
        {
            yield return new TestCaseData(new List<string>
            {
                "..123*...",
                "......180",
                "........."
            }, 22140);

            yield return new TestCaseData(new List<string>
            {
                "..123*...",
                "...543...",
                "........."
            }, 66789);

            yield return new TestCaseData(new List<string>
            {
                "..321*...",
                "...543...",
                "..678...."
            }, 174303);

            yield return new TestCaseData(new List<string>
            {
                "....3*...",
                "...543...",
                "..678....",
                "........."
            }, 1629);

            yield return new TestCaseData(new List<string>
            {
                "..100*...",
                "......500",
                "..678....",
            }, 50000);



            yield return new TestCaseData(new List<string>
            {
                "..123....",
                ".....*180",
                "........."
            }, 22140);

            yield return new TestCaseData(new List<string>
            {
                "..123....",
                "..*543...",
                "........."
            }, 66789);

            yield return new TestCaseData(new List<string>
            {
                "..321....",
                "..*543...",
                "..678...."
            }, 0);

            yield return new TestCaseData(new List<string>
            {
                ".....3...",
                "...543*..",
                "..678....",
                "........."
            }, 1629);

            yield return new TestCaseData(new List<string>
            {
                "..100...",
                ".....*500",
                "..678....",
            }, 0);
        }
    }
}