// Using the Assertion namespace.
using OxDED.Terminal.Assertion;
using OxDED.Terminal.Logging;

class Program {
    public static void Main(string[] args) {
        Logger logger = new(name:"Assertion", severity:Severity.Trace);

        /// Assert
        // Assert is used to begin an assertion like this:
        Assert.Is(4, 2).Log(logger);
        // Here an assertion is created which checks if 4 == 2. Spoiler alert: it doesn't.
        // NOTE: Log(...) is used to log an assertion to the terminal.

        /// Assertions
        // There are 4 types of assertions:
        //  - Value assertion     : Checks if values match (like 4, 2).
        //  - Type assertion      : checks if types match.
        //  - Exception assertion : Checks if an unhandled exception occured.
        //  - Reference assertion : Can check if an object is null.
        // Here is an exception assertion used:
        Assert.Throws(()=>throw new Exception("Test")).Log(logger);

        // Assertions contain methods to check or handle the result:
        Assert.Is(4, 2).Log(logger)
            .OnFailure((Assertion failedAssertion) => { // Gets executed when the result is not Success.
                logger.LogFatal("Panic!");
            })
            .IsSuccess(); // True if the result is Success.
        
        /// Asserters
        // Asserters basically do a assert of a pre-defined value:
        _ = 
            new Asserter<int>(42).IsType<int>().IsSuccess() // Asserter equivalent
                == 
            Assert.IsType<int>(42).IsSuccess();             // Assert   equivalent
        
        // Some custom asserter functions:
        new Asserter<bool>(true).IsTrue();
        new Asserter<Action>(()=>throw new Exception()).Throws();

        // Different assertions have different asserters.
        // Like the exception assertion has (ExceptionAssertion<T, TException> has another one):
        ExceptionAssertion<Exception>.Create(()=>{})
            .ExceptionAsserter();
        // And the value asserter has two:
        ValueAssertion<int, int>.Create(1, 2)
            .Asserter(); // For 1.
        ValueAssertion<int, int>.Create(1, 2)
            .SecondAsserter(); // For 2.
        // And others have other asserters.
        // NOTE: All assertions have ResultAsserter(), that asserts the result.

        // For all asserters in a assertion is a Get...() variant:
        ValueAssertion<int, int>.Create(1, 2)
            .ResultAsserter();                // Asserter
        ValueAssertion<int, int>.Create(1, 2)
            .GetResult();                     // Getter

        // Asserters can also change types:
        // This assertion first checks if 42 is an int then it asserts the type (int).
        // After that it converts the asserter into a string asserter.
        Assert.IsType<int>(42).TypeAsserter().Do((Type type) => type.Name).Log(logger);

        /// Put together
        // Assert that "1" is 1, when it fails, 
        //     convert the failed assertion back, log the first value: "1",
        // when it fails,
        //     convert the failed assertion back, log the second value: 1,
        Assert.Is("1", 1).OnFailure((Assertion failedAssertion) =>
            // NOTE: We already know that failedAssertion is ValueAssertion<string, int>.
            failedAssertion.As<ValueAssertion<string, int>>()!.Asserter().Log(logger)
        ).OnFailure((Assertion failedAssertion) => 
            failedAssertion.As<ValueAssertion<string, int>>()!.SecondAsserter().Log(logger)
        );


        Assert.IsTrue(false).Throw();
        // bye, bye.
    }
}