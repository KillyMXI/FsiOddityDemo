namespace B

open A

module Worker =
    let doWork x =
        sprintf "%i ^2 *2 = %i" x (x |> Squarer.sqr |> doubler.double)
