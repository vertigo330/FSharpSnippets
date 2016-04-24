#load "types.fsx"
open types
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

//Generics - Write a function that takes an option and returns the value
//Automatic Generalisation
//F# interactive evaluated this as :
//  "val readValue : opt:'a option -> 'a
let readValue opt = 
    match opt with
    | Some(v) -> v
    | None -> failwith "No value!"

//Higher order functions revisited
let nums = [4; 9; 1; 8; 6]
let evens = List.filter (fun x -> x % 2 = 0) nums

//Lambda syntax, partial function application, and function currying
let Add = fun x y -> x + y          //here we use the shorthand lamba syntax to def the function
let AddTen = Add 10                 //partially evaluate the function with one argument
let AddTenAndSeven = AddTen 7        //complete the partial evauation by providing the final argument

//Higher-order functions
let Twice = fun f x -> f(f(x))
let Sixteen = Twice (fun x -> x * x) 2

//More partial function application
let PartialTwice = Twice (fun x -> x + x)   //only provided one argument->returned a new function with a single arg requirement
let Eight = PartialTwice 2                  //then provided the remailing arg

let add = fun x y -> x + y
let SomeNumbers = [1..10]
let AddToToNums = List.map (add 1) SomeNumbers