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

//Now lets create another representation similar to the HTML standard.
type Orientation =
    | Vertical
    | Horizontal

type DocumentPart =
    | SplitPart of Orientation * list<DocumentPart>
    | TitlePart of TextContent * DocumentPart
    | TextPart of TextContent
    | ImagePart of string

//Now use the data structures to represent some data
let doc =   
    TitlePart ({ Text = "Functional Programming for the Real World"; 
                        Font = fntHead },
            SplitPart (Vertical, 
                [ImagePart("Cover.jpg");
                TextPart({ Text = "In this book there is a lot of words and stuff..."; Font=fntText})]
            )
        )

//Now, a function to convert from the first format to the second (projection?)
let rec documentToScreen(doc, bounds) =
    match doc with
    | SplitPart(Horizontal, parts) ->
        let width = bounds.Width / float32(parts.Length)
        parts
        |> List.mapi(fun i part -> 
            let left = bounds.Left + float32(i) * width
            let bounds = {bounds with Left = left; Width = width}
            documentToScreen(part, bounds))
        |> List.concat
    | SplitPart(Vertical, parts) ->
        let height = bounds.Height / float32(parts.Length)
        parts
        |> List.mapi(fun i part -> 
            let top = bounds.Top + float32(i) * height
            let bounds = {bounds with Top = top; Height = height}
            documentToScreen(part, bounds))
        |> List.concat
    | TitlePart(text, content) -> 
        let titleBounds = { bounds with Height = 35.0f}
        let restBounds = {bounds with Height = bounds.Height - 35.0f; Top = bounds.Top + 35.0f } 
        let convertedBody = documentToScreen(content, restBounds)
        TextElement(text, titleBounds) :: convertedBody
    | TextPart(text) -> [TextElement(text, bounds)]
    | ImagePart(img) -> [ImageElement(img, bounds)]

//Now project from XML format into the document structure
//But first...some utility functions...
open System.Xml.Linq

let attr (node: XElement, name, defaultValue) = 
    let attr = node.Attribute(XName.Get(name))
    if(attr <> null) then attr.Value else defaultValue

let parseOrientation(node) =
    match attr(node, "orientation", "") with
    | "horizontal" -> Horizontal
    | "vertical" -> Vertical
    | _ -> failwith "Unknown orientation!"

let parseFont(node) = 
    let style = attr(node, "style", "")
    let style = 
        match style.Contains("bold"), style.Contains("italic") with
        | true, false -> FontStyle.Bold
        | false, true -> FontStyle.Italic
        | true, true -> FontStyle.Bold ||| FontStyle.Italic
        | false, false -> FontStyle.Regular
    let name = attr(node, "font", "Calibri")
    new Font(name, float32(attr(node, "size", "12")), style)


#r "System.Xml.dll"
#r "System.Xml.Linq.dll"
let rec loadPart(node: System.Xml.Linq.XElement) =
    match node.Name.LocalName with
    | "titled" -> 
        let tx = { Text=attr(node, "title", ""); Font=parseFont node }
        let body = loadPart(Seq.head(node.Elements()))
        TitlePart(tx, body)
    | "split" ->
        let orientation = parseOrientation node
        let nodes = node.Elements() |> List.ofSeq |> List.map loadPart
        SplitPart(orientation, nodes)
    | "text" ->
        TextPart({Text = node.Value; Font = parseFont node})
    | "image" ->
        ImagePart(attr(node, "filename", ""))
    | name -> failwith("Unknown node: " + name)

//Now display the document on a windows form
open System.Windows.Forms

[<System.STAThread>]
do
    let doc = loadPart(XDocument.Load(@"C:\Projects\Playpen\MyFSharpLibrary\MyFSharpLibrary\document.xml").Root)
    let bounds = { Left = 0.0f; Top = 0.0f; Width=520.0f; Height=630.0f }
    let parts = documentToScreen(doc, bounds)
    let img = drawImage(570, 680) 25.0f (drawElements parts)
    let main = new Form(Text = "Document", BackGroundImage = img, ClientSize = Size(570, 680))

    Application.Run(main)