using System;
namespace Main {
    class CalcView : System.Object
    {
        public CalcView (
        ) {
        }
        public void DrawFrame() {
            Console.Clear();
            Console.WriteLine("-------------");
            Console.WriteLine("│            │");
            Console.WriteLine("└──────┘");
        } // DrawFrame
        public void Sleep() {
        } // Sleep
        public void WakeUp() {
        } // WakeUp
        public void Update( string entry) {
            Console.SetCursorPosition(2, 1);
            Console.Write("{0,12}", entry);
        } // Update

    }
}
