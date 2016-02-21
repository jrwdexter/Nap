namespace Nap

type Event<'T> =
    | ModifyingEvent of ('T -> FinalizableType<'T,'T>)
    | ActionEvent of ('T -> unit)
    with
    member x.RunEvent value = 
        match x with
        | ModifyingEvent(e) -> e value
        | ActionEvent(e) -> e value; Continuing(value)