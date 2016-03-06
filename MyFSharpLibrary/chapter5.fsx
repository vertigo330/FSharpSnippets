open System

//Use a tuple to return multiple values
let divide x y = 
    (x / y, x % y)
let res = divide 10 3

//Use tuples to call into .net out param functions
let (succeess, number) = Int32.TryParse("2");

//Tuple composition
let simple = (50, 100, "hello world")
let nested = ((50, 100), "hello world")

let printMessage (x, y) message = 
    printfn "[%d %d] %s" x y message

printMessage (fst(nested)) (snd(nested))

//Declare a discriminated union type
type Schedule = 
    | Never
    | Once of DateTime
    | Recurring of (DateTime * TimeSpan)

//Declaring a few values of Schdule type
let tomorrow = DateTime.Now.AddDays(1.0)
let noon = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0)
let daySpan = new TimeSpan(24, 0, 0)

let schedule1 = Schedule.Never
let schedule2 = Schedule.Once(tomorrow)
let schedule3 = Schedule.Recurring(noon, daySpan)

let getNextOccurance schedule =
    match schedule with
    | Never -> DateTime.MaxValue
    | Once(occuranceDate) -> 
        if occuranceDate > DateTime.Now then
            occuranceDate
        else
            DateTime.MaxValue
    | Recurring(firstOccurance, interval) ->
        let secondsFromFirstOccurance = (DateTime.Now - firstOccurance).TotalSeconds
        let numberOfOccurancesThusFar = floor (secondsFromFirstOccurance / interval.TotalSeconds)
        let nextInterval = numberOfOccurancesThusFar + 1.0
        let nextOccurance = firstOccurance.AddSeconds(nextInterval * interval.TotalSeconds)
        nextOccurance

//The Option type -> built in discriminated union that allows you to indicate that no value is returned
let readInput() =
    let s = Console.ReadLine()
    match Int32.TryParse(s) with
    |(true, parsed) -> Some(parsed)
    |_ -> None

//When we use the function, we have to use pattern matching, and thus the compiler will make sure that we cover all cases, including the None case
let testInput =
    let input = readInput()
    match input with
        |Some(parsed)->printfn "You entered a number->%d" parsed
        |None->printfn "Invalid number entered"

//We can write our own more restrictive option type using a discriminated union
type IntType =
    | IntSome of int
    | IntNone