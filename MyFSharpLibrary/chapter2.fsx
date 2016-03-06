open System
open System.Drawing

// Simple function value
let Add a b = 
    let res = a + b
    String.Format("{0} + {1} = {2}", a, b, res);

//Declarative style
let numbers = [ 1 .. 10 ]
let isOdd a = 
    a % 2 = 1
let square a = a * a

List.filter isOdd numbers
List.map square (List.filter isOdd numbers)

//More declarative using piping (Language oriented programming->terse + extended vocab)
let squared = 
    numbers
    |> List.filter isOdd
    |> List.map square

//Discriminated union type definition
type Shape =
    | Rectangle of Point * Point
    | Elipse of Point * Point
    | Composed of Shape * Shape

//Units of measure
[<Measure>]
type kmph
[<Measure>]
type mph

let maxSpeed = 120.0<kmph>
let actualSpeed = 110.0<mph>

if(actualSpeed > maxSpeed) then
    printf("Speeding!")
