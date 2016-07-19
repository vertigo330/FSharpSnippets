open System
open System.Drawing

type Rect =
    {
        Left : float32
        Top : float32
        Width : float32
        Height : float32
    }

let deflate (original, wspace, hspace) =
    {
        Left = original.Left + wspace
        Top = original.Top + hspace
        Width = original.Width - (2.0f * wspace)
        Height = original.Height - (2.0f * hspace)
    }

let toRectangleF(original) = 
    RectangleF(original.Left, original.Top, original.Width, original.Height)

type TextContent = 
    {   
        Text : string
        Font : Font  
    }

type ScreenElement =
    | TextElement of TextContent * Rect
    | ImageElement of string * Rect

let fntText = new Font("Calibri", 12.0f)
let fntHead = new Font("Calibri", 15.0f)

let elements = 
    [ TextElement
        ({ Text = "Functional Programming for the Real World"
           Font = fntHead },
         { Left = 10.0f; Top =  0.0f; Width = 410.0f; Height = 30.0f });
    ImageElement
        ( "Cover.jpg",
         { Left = 120.0f; Top =  30.0f; Width = 150.0f; Height = 200.0f }); 
    TextElement
        ({ Text = "In this book blah blah blah..."
           Font = fntText },
         { Left = 10.0f; Top =  230.0f; Width = 400.0f; Height = 400.0f }) ]