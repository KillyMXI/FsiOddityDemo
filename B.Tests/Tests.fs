module Tests

open Xunit
open B

[<Fact>]
let ``Test function with project dependency`` () =
    Assert.Equal("5 ^2 *2 = 50", (Worker.doWork 5))
