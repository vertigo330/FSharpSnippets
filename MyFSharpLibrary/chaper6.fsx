open System

//Generic higher order functions
let TestAndPrint = fun num test format ->
    if(test(num)) then printfn "%s" (format(num))

TestAndPrint 5 
            (fun num -> num < 10) 
            (fun num -> "Number is " + num.ToString())

//Custom operators
let (+>) a b = a + "\n->" + b

printfn "%s" ("Hello" +> "World" +> "Aint" +> "Life" +> "Grand")

//The pipelining operator
[1..10] |> List.rev |> List.head

//Lets pipe some stuff into our own functions
let prague = ("Prague", 125000)
let mapSecond = fun f (a, b) -> (a, f(b))

//prague |> mapSecond returns a partially evaluated function, we still need to provide the f, which is the  ((+) 1000) part
prague |> mapSecond ((+) 1000)

//Playing with this idea a bit
let Five = 5
let TimesTwo = fun x -> x * 2
let PlusThree = fun x -> x + 3

Five 
    |> TimesTwo 
    |> PlusThree

//Working with the sheduler from ch5
#load "common.fsx"
open common

let schedules = [Never; 
                Once(DateTime(2016, 4, 17)); 
                Recurring(DateTime(2016, 4, 17), TimeSpan(24 * 7, 0, 0))]

let adjustSchedules f s =
    match s with
        |Never -> Never
        |Once(dt) -> Once(f(dt))
        |Recurring(dt, ts) -> Recurring(f(dt), ts)


schedules |> List.map (adjustSchedules (fun dt -> dt.AddDays(7.0)))

//higher order functions and the option type
let addNumbers = 
    match readInput() with
    | None -> None
    | Some(i) -> 
        match readInput() with
        | None -> None
        | Some(j) -> Some(i + j)

// Using Option.map
// (('a -> 'b) -> 'a option -> 'b option)
let addNumbersMap =
    match readInput() with
    | None -> None
    | Some(i) -> readInput() |> Option.map(fun j -> i + j)

// Using Option.bind to replace the outer match
// (('a -> 'b option) -> 'a option -> 'b option)
let addNumbersBind = 
    readInput() |> Option.bind (fun first -> readInput() |> Option.map(fun second -> first + second))

//Fuction composition
let statusByPopulation = fun population -> 
    match population with
    | n when n > 1000000 -> "City"
    | n when n > 5000 -> "Town"
    | _ -> "Village"

let places = [("Johannesburg", 1180152);
              ("Krugersdorp", 10521);
              ("Hillsborough", 230) ]

places |> List.map(snd >> statusByPopulation)