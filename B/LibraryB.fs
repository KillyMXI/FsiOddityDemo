namespace B

open A

module Worker =
    let doWork x =
        sprintf "%i * 2 = %i" x (doubler.double x)
