module common
open System

//My first discriminated union type!
type Schedule = 
    | Never
    | Once of DateTime
    | Recurring of (DateTime * TimeSpan)


let readInput() =
    let s = Console.ReadLine()
    match Int32.TryParse(s) with
    |(true, parsed) -> Some(parsed)
    |_ -> None
