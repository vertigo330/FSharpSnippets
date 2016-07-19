open System

//The record data type -> just a labelled tuple
type Rectangle = 
    {
        Left: float32
        Top: float32
        Width: float32
        Height: float32
    }

let rc1 = { Left = 10.0f; Top = 10.0f; Width = 100.0f; Height = 200.0f }

//Because this is an immutable type, we cant just change the values, we need to create a new one with the modified values
//F# allows us to do this in a succinc manner using "With" keyword

let rc2 = { rc1 with Left = rc1.Left + 100.0f}

//Now some functions to manipulate Rectangles
//This deflates the rectange by adjusting the left, top, width and height
let deflate(original, wspace, hspace) =
    {
        Left = original.Left + wspace
        Top = original.Top + hspace
        Width = original.Width - (2.0f * wspace)
        Height = original.Height - (2.0f * hspace)
    }

//This converts the rectange into a System.Drawing.RectangleF
let toRectange(original) =
    System.Drawing.RectangleF(original.Left, original.Top, original.Width, original.Height)