namespace C

open A

module Doer =
    let doStuff x =
        sprintf "%i *2 = %i" x (x |> doubler.double)
