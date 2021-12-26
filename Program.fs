//This module's imported namespaces.
open System
open System.Diagnostics
open System.Drawing
open System.Globalization
open System.Reflection
open System.Threading
open System.Windows.Forms

//The global objects and variables used by this module.
let ProgramInformation = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location)   //Contains this program's information.

//This module contains this program's main interface class.
module private InterfaceModule =
   let TO_RADIANS = 180.0 / Math.PI   //This value is used to convert degrees to radians.

   //This class contains this program's main interface window.
   type private InterfaceWindow() as Form = 
      inherit Form()
      do Form.InitializeForm

      //This procedure initializes this window.
      member this.InitializeForm = 
         this.BackColor <- Color.Black 
         this.Height <- (((Screen.PrimaryScreen.WorkingArea.Height |> float) / 1.1) |> int)
         this.Paint.AddHandler(new PaintEventHandler (fun s e -> this.Form_Paint(s, e)))
         this.Resize.AddHandler(new EventHandler (fun s e -> this.Form_Resize(s, e)))
         this.StartPosition <- FormStartPosition.CenterScreen 
         this.Text <- String.Format("{0} v{1} - by: {2}", ProgramInformation.ProductName, ProgramInformation.ProductVersion, ProgramInformation.CompanyName)
         this.Width <- (((Screen.PrimaryScreen.WorkingArea.Width |> float) / 1.1) |> int)      

      //This procedure draws a figure.
      member this.Form_Paint(sender:Object, e:PaintEventArgs) = 
         let centerX = (this.ClientRectangle.Width |> float) / 2.0
         let centerY = (this.ClientRectangle.Height |> float) / 2.0
         
         let rec drawFigure (radii:List<float>) (angle:float) (interval:float) = 
            if List.isEmpty(radii) then
               ()
            else
               let x = ((Math.Sin(angle / TO_RADIANS) * radii.Head) + centerX) |> int
               let y = ((Math.Cos(angle / TO_RADIANS) * radii.Head) + centerY) |> int
               let nextX = ((Math.Sin((angle + interval) / TO_RADIANS) * (if radii.Length = 1 then radii.Head else radii.Tail.Head)) + centerX) |> int
               let nextY = ((Math.Cos((angle + interval) / TO_RADIANS) * (if radii.Length = 1 then radii.Head else radii.Tail.Head)) + centerY) |> int
               
               e.Graphics.DrawLine(Pens.Red, x, y, nextX, nextY)

               drawFigure radii.Tail (angle + interval) interval

         drawFigure [300.0; 300.0; 300.0] 0.0 120.0
         
      //This procedure adjusts this window's objects to its new size.
      member this.Form_Resize(sender:Object, e:EventArgs) = 
         this.Invalidate()

   //This procedure is executed when this program is started.
   [<STAThread>]
   do Application.Run(new InterfaceWindow())
