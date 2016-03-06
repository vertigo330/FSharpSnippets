open System

//A nested function value
let printSquares message num1 num2 =
    let printSquareUtility num = 
        let squared = num  * num
        printfn "%s %d: %d" message num squared
    printSquareUtility num1
    printSquareUtility num2

printSquares "Squared number is..." 5 7

//Tuples and pattern matching
let printCity cityTuple =
    printfn "City of %s has a population of %d" (fst cityTuple) (snd cityTuple)

let withNewValue newValue tuple =
    let (val1, _) = tuple    //This decomposes the tuple so that we can reuse the first element from it
    (val1, newValue)

let prague = ("Prague", 1188000)
let pragueNew = withNewValue (snd(prague) + 13195) prague

printCity prague
printCity pragueNew

//Another example of pattern matching
let setPopulation tuple population =
    match tuple with
    | ("New York", _) -> ("New York", population + 100)
    | (city, _) -> (city, population)

let prague = setPopulation ("Prague", 123) 1000
let newYork = setPopulation ("New York", 250) 500
printCity prague
printCity newYork

//Recursion, and recursive functions
let rec factorial(n) =
    if(n <= 1)then
        1
    else
        n * factorial(n - 1)

//Recursive lists
let lst1 = [1..10]
let lst2 = [11; 12]
let lst3 = 13::14::15::[]
let lst4 = 0::lst2

//Recursive list processing
let rec sumList(lst) = 
    match lst with
    | [] -> 0
    | [con::cell] -> con + sumList(cell)