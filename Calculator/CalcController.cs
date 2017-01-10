using System;
using Main/*.CalcModel*/;
namespace Main {
    class CalcController : System.Object
    {
        CalcModel _calcModel;                                                                                                   /*<  */
        public CalcController (
        ) {
            _calcModel = new CalcModel();                                                                                       /*<  */
        }
        public void Run() {
            _calcModel.RunToCompletion();
            char key = '\0';
            do {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                key = keyInfo.KeyChar;
                switch (Char.ToLower(key)) {
                case '0':
                    _calcModel.Digit0();
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    _calcModel.Digit19(key);
                    break;
                case 'a':
                    _calcModel.Ac();
                    break;
                case 'c':
                    _calcModel.Ce();
                    break;
                case '+':
                case '-':
                case '*':
                case '/':
                    _calcModel.Oper(key);
                    break;
                case '.':
                    _calcModel.Point();
                    break;
                case 'q':
                    _calcModel.Off();
                    break;
                case '=':
                    _calcModel.Equals();
                    break;
                default:
                    break;
                }
            } while (key != 'q');
        } // Run

    }
}
