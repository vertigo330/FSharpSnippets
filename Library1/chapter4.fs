//Windows form
open System
open System.IO
open System.Drawing
open System.Windows.Forms

let mainForm = new System.Windows.Forms.Form(Width=620, Height=420, Text="Pie Chart")
let menu = new ToolStrip()
let btnOpen = new ToolStripButton("Open")
let btnSave = new ToolStripButton("Save", Enabled=false)

ignore(menu.Items.Add(btnOpen))
ignore(menu.Items.Add(btnSave))

let boxChart = new PictureBox(BackColor=Color.White, Dock=DockStyle.Fill, SizeMode=PictureBoxSizeMode.CenterImage)

mainForm.Controls.Add(menu)
mainForm.Controls.Add(boxChart)

let rnd = new Random()
let randomBrush() = 
    let r, g, b = rnd.Next(256), rnd.Next(256), rnd.Next(256)
    new SolidBrush(Color.FromArgb(r, g, b))

let drawPieSegment(gr:Graphics, title, startAngle, occupiedAngle) = 
    let br = randomBrush()
    gr.FillPie(br, 170, 70, 260, 260, startAngle, occupiedAngle)
    br.Dispose()


let fnt = new Font("Times New Roman", 11.0f)
let centerX, centerY = 300.0, 200.0
let labelDistance = 150.0

let drawLabel(gr:Graphics, title, startAngle, angle) = 
    let lblAngle = float(startAngle + angle/2)
    let ra = Math.PI * 2.0 * lblAngle / 360.0
    let x = centerX + labelDistance * cos(ra)
    let y = centerY + labelDistance * sin(ra)
    let size = gr.MeasureString(title, fnt)
    let rc = new PointF(float32(x) - size.Width / 2.0f, float32(y) - size.Height / 2.0f)
    
    gr.DrawString(title, fnt, Brushes.Black, new RectangleF(rc, size))

let convertDataRow(csvLine:string) = 
    let cells = List.ofSeq(csvLine.Split(','))
    match cells with
    | title::number::_->
        let parsedNumber = Int32.Parse(number)
        (title, parsedNumber)
    | _-> failwith "Invalid data format"

//Process a List of strings , converting each to a tuple
let rec processLines(lines) = 
    match lines with
    | []->[]  //Empty list
    | currentLine::remaining ->
        let parsedLines = convertDataRow(currentLine)
        let parsedRest = processLines(remaining)
        parsedLines::parsedRest

let rec calculateSum records =
    match records with
    | []->0
    | (_, value)::tail->
        let total = value + calculateSum(tail)
        total

let drawStep(drawingFunc, gr:Graphics, sum, data) =
    let rec drawStepUtil(data, angleSoFar) =
        match data with
        | [] -> ()
        | [title, value] ->
            let angle = 360 - angleSoFar
            drawingFunc(gr, title, angleSoFar, angle)
        | (title, value) :: tail ->
            let angle = int(float(value) / sum * 360.0)
            drawingFunc(gr, title, angleSoFar, angle)
            drawStepUtil(tail, angleSoFar + angle)
    drawStepUtil(data, 0)

let drawChart(file) = 
    let lines = List.ofSeq(File.ReadAllLines(file))
    let data = processLines(lines)
    let sum = float(calculateSum(data))
    let pieChart = new Bitmap(600, 400)
    let gr = Graphics.FromImage(pieChart)

    gr.Clear(Color.White)
    drawStep(drawPieSegment, gr, sum, data)
    drawStep(drawLabel, gr, sum, data)

    gr.Dispose();
    pieChart

let openAndDrawChart(e) = 
    let dlg = new OpenFileDialog(Filter = "CSV Files|*.csv")
    if(dlg.ShowDialog() = DialogResult.OK) then
        let pieChart = drawChart(dlg.FileName)
        boxChart.Image <- pieChart
        btnSave.Enabled <- true

let saveDrawing(e) =
    let dlg = new SaveFileDialog(Filter = "PNG Files|*.png")
    if(dlg.ShowDialog() = DialogResult.OK) then
        boxChart.Image.Save(dlg.FileName)

[<STAThread>]
do
    btnOpen.Click.Add(openAndDrawChart)
    btnSave.Click.Add(saveDrawing)
    Application.Run(mainForm)