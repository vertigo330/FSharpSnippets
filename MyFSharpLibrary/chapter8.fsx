#load "common.fsx"
open System
open common

let john = 
    {
        Name = "John Doe"
        Income = 40000
        YearsInJob = 1
        UsesCreditCard = true
        CriminalRecord = false
    }

let tests = 
    [
        fun cl -> cl.CriminalRecord
        fun cl -> cl.Income < 30000
        fun cl -> not cl.UsesCreditCard
        fun cl -> cl.YearsInJob < 2
    ]

let testClient(client) =
    let issues = tests |> List.filter(fun f -> client |> f)
    let suitable = issues.Length <= 1
    printfn "Client: %s\nOffer a Loan: %s (Issues %d)" client.Name
        (if (suitable) then "YES" else "NO") issues.Length

john |> testClient

//An F# closure!
let adder num = fun m -> m + num

//Partial function application
let addFive = adder 5

//Rock n roll functional programming! |m|
addFive 10

//ref cells -> There are mutable!
let rc = ref 10

rc := 15

printfn "%d" (!rc)

//So now we create a closure that captures the mutable value
let createIncomeTests() =
    let minimumIncome = ref 30000
    (fun (newIncome)->
        minimumIncome := newIncome),
    (fun (client)->
        client.Income < (!minimumIncome))

//These 2 functions are returned as a tuple from the createIncomeTests function
let setMinIncome, earnsTooLittle = createIncomeTests()

let Bob = 
    {
        Name = "John Doe"
        Income = 40000
        YearsInJob = 1
        UsesCreditCard = true
        CriminalRecord = false
    }

Bob |> earnsTooLittle

//This line mutates the state captured by the closure to change the minimum income
45000 |> setMinIncome

//Now when we run the same test with the same parameter, the result is different. Like magic!
Bob |> earnsTooLittle

//Use a record type to represent a named list of functions
type ClientTest =
    { Check : Customer -> bool
      Report : Customer -> unit }

let checkCriminal(client) = client.CriminalRecord = true
let reportCriminal(client) = printfn "Checking 'Criminal Record' of '%s' failed!" client.Name

let checkIncome(client) = client.Income < 30000
let reportIncome(client) = printfn "Checking 'Income' of '%s' failed! Less then 30000" client.Name

let checkJobYears(client) = client.YearsInJob < 2
let reportJobYears(client) = printfn "Checking 'Job Years' of '%s' failed! Less then 2" client.Name

let testsWithReport = 
    [ {Check = checkCriminal; Report = reportCriminal };
      {Check = checkIncome; Report = reportIncome };
      {Check = checkJobYears; Report = reportJobYears }; ]

let testClientWithReports(client) =
    let issues = testsWithReport |> List.filter(fun f->f.Check(client))
    let suitable = issues.Length <= 1
    for i in issues do
        i.Report(client)
        printfn "Offer loan %s"
            (if (suitable) then "YES" else "NO" )

testClientWithReports(Bob) 