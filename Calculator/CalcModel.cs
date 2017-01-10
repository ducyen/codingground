using System;
using Main;
namespace Main {
    class CalcModel : System.Object
    {
        protected class OPER_PARAMS {
            public char key;
        };
        protected class DIGIT_1_9_PARAMS {
            public char digit;
        };
        Statemachine _stm;                                                                                                      /*<  */
        string _intEntry;                                                                                                       /*<  */
        string _fracEntry;                                                                                                      /*<  */
        char _signEntry;                                                                                                        /*<  */
        char _operEntry;                                                                                                        /*<  */
        double _operand1;                                                                                                       /*<  */
        CalcView _calcView;                                                                                                     /*<  */
        public CalcModel (
        ) {
            _stm = new Statemachine(null);                                                                                      /*<  */
            _intEntry = "";                                                                                                     /*<  */
            _fracEntry = "";                                                                                                    /*<  */
            _signEntry = ' ';                                                                                                   /*<  */
            _operEntry = ' ';                                                                                                   /*<  */
            _operand1 = 0f;                                                                                                     /*<  */
            _calcView = new CalcView();                                                                                         /*<  */
        }
        private void insertInt( char digit) {
            _intEntry = _intEntry + digit;
            _calcView.Update(entry);
        } // insertInt
        private void insertFrac( char digit) {
            _fracEntry = _fracEntry + digit;
            _calcView.Update(entry);
        } // insertFrac
        private void negate() {
            _signEntry = '-';
            _calcView.Update(entry);
        } // negate
        private void set( char digit) {
            _intEntry = digit.ToString();
            _fracEntry = "";
            _calcView.Update(entry);
        } // set
        private void clear() {
            _signEntry = ' ';
            set( '0' );
        } // clear
        private void setOper( char key) {
            _operEntry = key;
            _calcView.Update(entry);
        } // setOper
        private bool calculate() {
            bool error = false;
            switch (_operEntry) {
            case '+': _operand1 = _operand1 + curOperand; break;
            case '-': _operand1 = _operand1 - curOperand; break;
            case '*': _operand1 = _operand1 * curOperand; break;
            case '/':
                if (curOperand == 0f) {
                    error = true;
                } else {
                    _operand1 = _operand1 / curOperand;
                }
                break;
            }
            return error;
        } // calculate
        private void showResult() {
            string result = _operand1.ToString("0.########");
            int dotPos = result.IndexOf(".");
            string intRslt = "0";
            string fracRslt = "";
            if (dotPos >= 0) {
                intRslt = result.Substring(0, dotPos);
                fracRslt = result.Substring(dotPos + 1);
            } else {
                intRslt = result.Substring(0);
            }
            _calcView.Update(intRslt + "." + fracRslt + _operEntry);
        } // showResult
        private void showError() {
            _calcView.Update("Err" + _operEntry);
        } // showError
        public string entry {
            get {
                return _signEntry + _intEntry + "." + _fracEntry + _operEntry;
            }
        }
        public double curOperand {
            get {
                return double.Parse(_signEntry + _intEntry + "." + _fracEntry);
            }
        }
        const UInt32 RESULT = 1;
        const UInt32 BEGIN = 2;
        const UInt32 NEGATED1 = 4;
        const UInt32 ZERO1 = 8;
        const UInt32 INT1 = 16;
        const UInt32 FRAC1 = 32;
        const UInt32 ERROR = 64;
        const UInt32 OP_ENTERED = 128;
        const UInt32 NEGATED2 = 256;
        const UInt32 FRAC2 = 512;
        const UInt32 INT2 = 1024;
        const UInt32 ZERO2 = 2048;
        const UInt32 STM = UInt32.MaxValue;
        const UInt32 ON = ( READY | NEGATED1 | OPERAND1 | ERROR | OP_ENTERED | NEGATED2 | OPERAND2 );
        const UInt32 READY = ( RESULT | BEGIN );
        const UInt32 OPERAND1 = ( ZERO1 | INT1 | FRAC1 );
        const UInt32 OPERAND2 = ( FRAC2 | INT2 | ZERO2 );
        protected enum CALC_MODEL_EVENT {
            AC,
            CE,
            DIGIT_0,
            DIGIT_1_9,
            EQUALS,
            OFF,
            OPER,
            POINT
        };
        void EndTrans( bool isFinished ){
            _stm.nCurrentState = _stm.nTargetState;
            _stm.bIsFinished = isFinished;
            _stm.bIsExternTrans = false;
            if( isFinished ){
                return;
            }
            switch( _stm.nCurrentState ){
            case ON:            On_Entry(); break;
            case READY:         Ready_Entry(); break;
            case RESULT:        Result_Entry(); break;
            case BEGIN:         Begin_Entry(); break;
            case NEGATED1:      Negated1_Entry(); break;
            case OPERAND1:      Operand1_Entry(); break;
            case ZERO1:         Zero1_Entry(); break;
            case INT1:          Int1_Entry(); break;
            case FRAC1:         Frac1_Entry(); break;
            case ERROR:         Error_Entry(); break;
            case OP_ENTERED:    OpEntered_Entry(); break;
            case NEGATED2:      Negated2_Entry(); break;
            case OPERAND2:      Operand2_Entry(); break;
            case FRAC2:         Frac2_Entry(); break;
            case INT2:          Int2_Entry(); break;
            case ZERO2:         Zero2_Entry(); break;
            default: break;
            }
        }
        void BgnTrans( UInt32 sourceState, UInt32 targetState ){
            _stm.nSourceState = sourceState;
            _stm.nTargetState = targetState;
            switch( _stm.nCurrentState ){
            case ON:            On_Exit(); break;
            case READY:         Ready_Exit(); break;
            case RESULT:        Result_Exit(); break;
            case BEGIN:         Begin_Exit(); break;
            case NEGATED1:      Negated1_Exit(); break;
            case OPERAND1:      Operand1_Exit(); break;
            case ZERO1:         Zero1_Exit(); break;
            case INT1:          Int1_Exit(); break;
            case FRAC1:         Frac1_Exit(); break;
            case ERROR:         Error_Exit(); break;
            case OP_ENTERED:    OpEntered_Exit(); break;
            case NEGATED2:      Negated2_Exit(); break;
            case OPERAND2:      Operand2_Exit(); break;
            case FRAC2:         Frac2_Exit(); break;
            case INT2:          Int2_Exit(); break;
            case ZERO2:         Zero2_Exit(); break;
            default: break;
            }
        }
        public bool RunToCompletion(){
            bool bResult;
            do{
                if( Statemachine.IsComposite( _stm.nCurrentState ) && !_stm.bIsFinished ){
                    bResult = Start();
                }else{
                    bResult = Done();
                }
            }while( bResult );
            return bResult;
        }
        public bool IsFinished() {
            return _stm.bIsFinished && _stm.nCurrentState == STM;
        }
        bool EventProc( UInt32 nEventId, Object pEventParams ){
            bool bResult = false;
            switch( _stm.nCurrentState ){
            case ON:            bResult = On_EventProc( nEventId, pEventParams ); break;
            case READY:         bResult = Ready_EventProc( nEventId, pEventParams ); break;
            case RESULT:        bResult = Result_EventProc( nEventId, pEventParams ); break;
            case BEGIN:         bResult = Begin_EventProc( nEventId, pEventParams ); break;
            case NEGATED1:      bResult = Negated1_EventProc( nEventId, pEventParams ); break;
            case OPERAND1:      bResult = Operand1_EventProc( nEventId, pEventParams ); break;
            case ZERO1:         bResult = Zero1_EventProc( nEventId, pEventParams ); break;
            case INT1:          bResult = Int1_EventProc( nEventId, pEventParams ); break;
            case FRAC1:         bResult = Frac1_EventProc( nEventId, pEventParams ); break;
            case ERROR:         bResult = Error_EventProc( nEventId, pEventParams ); break;
            case OP_ENTERED:    bResult = OpEntered_EventProc( nEventId, pEventParams ); break;
            case NEGATED2:      bResult = Negated2_EventProc( nEventId, pEventParams ); break;
            case OPERAND2:      bResult = Operand2_EventProc( nEventId, pEventParams ); break;
            case FRAC2:         bResult = Frac2_EventProc( nEventId, pEventParams ); break;
            case INT2:          bResult = Int2_EventProc( nEventId, pEventParams ); break;
            case ZERO2:         bResult = Zero2_EventProc( nEventId, pEventParams ); break;
            default: break;
            }
            return bResult;
        }
        void On_Entry(){
            if( Statemachine.IsIn( STM, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                _stm.nLCAState = 0;
            }
        }
        bool On_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.AC:{
                _stm.bIsExternTrans = true;
                BgnTrans( ON, ON );
                _operEntry = ' ';
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.OFF:{
                BgnTrans( ON, STM );
                EndTrans( true );
                bResult = true;
            } break;
            default: break;
            }
            return bResult;
        }
        void On_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, ON ) && Statemachine.IsIn( _stm.nTargetState, ON );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
            } else { 
                _stm.nLCAState = ON;
            }
        }
        void Ready_Entry(){
            if( Statemachine.IsIn( ON, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                On_Entry();
                _signEntry = ' ';
                _stm.nLCAState = 0;
            }
        }
        bool Ready_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                BgnTrans( READY, ZERO1 );
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                BgnTrans( READY, INT1 );
                set(e.digit);
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( READY, FRAC1 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : On_EventProc( nEventId, pEventParams );
        }
        void Ready_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, READY ) && Statemachine.IsIn( _stm.nTargetState, READY );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                On_Exit();
            } else { 
                _stm.nLCAState = READY;
            }
        }
        void Result_Entry(){
            if( Statemachine.IsIn( READY, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Ready_Entry();
                _stm.nLCAState = 0;
            }
        }
        bool Result_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            return bResult ? bResult : Ready_EventProc( nEventId, pEventParams );
        }
        void Result_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, RESULT ) && Statemachine.IsIn( _stm.nTargetState, RESULT );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Ready_Exit();
            } else { 
                _stm.nLCAState = RESULT;
            }
        }
        void Begin_Entry(){
            if( Statemachine.IsIn( READY, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Ready_Entry();
                clear();
                _stm.nLCAState = 0;
            }
        }
        bool Begin_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.OPER:{
                OPER_PARAMS e = ( OPER_PARAMS )pEventParams;
                if (e.key == '-') {
                    BgnTrans( BEGIN, NEGATED1 );
                    EndTrans( false );
                    bResult = true;
                }
            } break;
            default: break;
            }
            return bResult ? bResult : Ready_EventProc( nEventId, pEventParams );
        }
        void Begin_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, BEGIN ) && Statemachine.IsIn( _stm.nTargetState, BEGIN );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Ready_Exit();
            } else { 
                _stm.nLCAState = BEGIN;
            }
        }
        void Negated1_Entry(){
            if( Statemachine.IsIn( ON, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                On_Entry();
                negate();
                _stm.nLCAState = 0;
            }
        }
        bool Negated1_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.CE:{
                BgnTrans( NEGATED1, BEGIN );
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_0:{
                BgnTrans( NEGATED1, ZERO1 );
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                BgnTrans( NEGATED1, INT1 );
                set(e.digit);
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( NEGATED1, FRAC1 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : On_EventProc( nEventId, pEventParams );
        }
        void Negated1_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, NEGATED1 ) && Statemachine.IsIn( _stm.nTargetState, NEGATED1 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                On_Exit();
            } else { 
                _stm.nLCAState = NEGATED1;
            }
        }
        void Operand1_Entry(){
            if( Statemachine.IsIn( ON, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                On_Entry();
                _stm.nLCAState = 0;
            }
        }
        bool Operand1_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.CE:{
                BgnTrans( OPERAND1, READY );
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.OPER:{
                OPER_PARAMS e = ( OPER_PARAMS )pEventParams;
                BgnTrans( OPERAND1, OP_ENTERED );
                _operand1 = curOperand;
                setOper( e.key );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : On_EventProc( nEventId, pEventParams );
        }
        void Operand1_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, OPERAND1 ) && Statemachine.IsIn( _stm.nTargetState, OPERAND1 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                On_Exit();
            } else { 
                _stm.nLCAState = OPERAND1;
            }
        }
        void Zero1_Entry(){
            if( Statemachine.IsIn( OPERAND1, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Operand1_Entry();
                set( '0' );
                _stm.nLCAState = 0;
            }
        }
        bool Zero1_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                ;
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                BgnTrans( ZERO1, INT1 );
                set(e.digit);
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( ZERO1, FRAC1 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : Operand1_EventProc( nEventId, pEventParams );
        }
        void Zero1_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, ZERO1 ) && Statemachine.IsIn( _stm.nTargetState, ZERO1 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Operand1_Exit();
            } else { 
                _stm.nLCAState = ZERO1;
            }
        }
        void Int1_Entry(){
            if( Statemachine.IsIn( OPERAND1, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Operand1_Entry();
                _stm.nLCAState = 0;
            }
        }
        bool Int1_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                insertInt('0');
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                insertInt(e.digit);
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( INT1, FRAC1 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : Operand1_EventProc( nEventId, pEventParams );
        }
        void Int1_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, INT1 ) && Statemachine.IsIn( _stm.nTargetState, INT1 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Operand1_Exit();
            } else { 
                _stm.nLCAState = INT1;
            }
        }
        void Frac1_Entry(){
            if( Statemachine.IsIn( OPERAND1, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Operand1_Entry();
                _stm.nLCAState = 0;
            }
        }
        bool Frac1_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                insertFrac('0');
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                insertFrac(e.digit);
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : Operand1_EventProc( nEventId, pEventParams );
        }
        void Frac1_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, FRAC1 ) && Statemachine.IsIn( _stm.nTargetState, FRAC1 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Operand1_Exit();
            } else { 
                _stm.nLCAState = FRAC1;
            }
        }
        void Error_Entry(){
            if( Statemachine.IsIn( ON, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                On_Entry();
                showError();
                _stm.nLCAState = 0;
            }
        }
        bool Error_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            return bResult ? bResult : On_EventProc( nEventId, pEventParams );
        }
        void Error_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, ERROR ) && Statemachine.IsIn( _stm.nTargetState, ERROR );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                On_Exit();
            } else { 
                _stm.nLCAState = ERROR;
            }
        }
        void OpEntered_Entry(){
            if( Statemachine.IsIn( ON, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                On_Entry();
                _signEntry = ' ';
                _stm.nLCAState = 0;
            }
        }
        bool OpEntered_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                BgnTrans( OP_ENTERED, ZERO2 );
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                BgnTrans( OP_ENTERED, INT2 );
                set(e.digit);
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.OPER:{
                OPER_PARAMS e = ( OPER_PARAMS )pEventParams;
                if (e.key == '-') {
                    BgnTrans( OP_ENTERED, NEGATED2 );
                    EndTrans( false );
                    bResult = true;
                }
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( OP_ENTERED, FRAC2 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : On_EventProc( nEventId, pEventParams );
        }
        void OpEntered_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, OP_ENTERED ) && Statemachine.IsIn( _stm.nTargetState, OP_ENTERED );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                On_Exit();
            } else { 
                _stm.nLCAState = OP_ENTERED;
            }
        }
        void Negated2_Entry(){
            if( Statemachine.IsIn( ON, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                On_Entry();
                clear();
                negate();
                _stm.nLCAState = 0;
            }
        }
        bool Negated2_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.CE:{
                BgnTrans( NEGATED2, OP_ENTERED );
                clear();
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_0:{
                BgnTrans( NEGATED2, ZERO2 );
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                BgnTrans( NEGATED2, INT2 );
                set(e.digit);
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( NEGATED2, FRAC2 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : On_EventProc( nEventId, pEventParams );
        }
        void Negated2_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, NEGATED2 ) && Statemachine.IsIn( _stm.nTargetState, NEGATED2 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                On_Exit();
            } else { 
                _stm.nLCAState = NEGATED2;
            }
        }
        void Operand2_Entry(){
            if( Statemachine.IsIn( ON, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                On_Entry();
                _stm.nLCAState = 0;
            }
        }
        bool Operand2_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.CE:{
                BgnTrans( OPERAND2, OP_ENTERED );
                clear();
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.EQUALS:{
                bool error = calculate();
                if (error) {
                    BgnTrans( OPERAND2, ERROR );
                    EndTrans( false );
                    bResult = true;
                } else {
                    BgnTrans( OPERAND2, RESULT );
                    _operEntry = ' ';
                    showResult();
                    EndTrans( false );
                    bResult = true;
                }
            } break;
            case CALC_MODEL_EVENT.OPER:{
                OPER_PARAMS e = ( OPER_PARAMS )pEventParams;
                bool error = calculate();
                setOper( e.key );
                if (error) {
                    BgnTrans( OPERAND2, ERROR );
                    EndTrans( false );
                    bResult = true;
                } else {
                    BgnTrans( OPERAND2, OP_ENTERED );
                    showResult();
                    EndTrans( false );
                    bResult = true;
                }
            } break;
            default: break;
            }
            return bResult ? bResult : On_EventProc( nEventId, pEventParams );
        }
        void Operand2_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, OPERAND2 ) && Statemachine.IsIn( _stm.nTargetState, OPERAND2 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                On_Exit();
            } else { 
                _stm.nLCAState = OPERAND2;
            }
        }
        void Frac2_Entry(){
            if( Statemachine.IsIn( OPERAND2, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Operand2_Entry();
                _stm.nLCAState = 0;
            }
        }
        bool Frac2_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                insertFrac('0');
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                insertFrac(e.digit);
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : Operand2_EventProc( nEventId, pEventParams );
        }
        void Frac2_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, FRAC2 ) && Statemachine.IsIn( _stm.nTargetState, FRAC2 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Operand2_Exit();
            } else { 
                _stm.nLCAState = FRAC2;
            }
        }
        void Int2_Entry(){
            if( Statemachine.IsIn( OPERAND2, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Operand2_Entry();
                _stm.nLCAState = 0;
            }
        }
        bool Int2_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                insertInt('0');
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                insertInt(e.digit);
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( INT2, FRAC2 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : Operand2_EventProc( nEventId, pEventParams );
        }
        void Int2_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, INT2 ) && Statemachine.IsIn( _stm.nTargetState, INT2 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Operand2_Exit();
            } else { 
                _stm.nLCAState = INT2;
            }
        }
        void Zero2_Entry(){
            if( Statemachine.IsIn( OPERAND2, _stm.nLCAState ) || _stm.nLCAState == 0 ){
                Operand2_Entry();
                set( '0' );
                _stm.nLCAState = 0;
            }
        }
        bool Zero2_EventProc(
            UInt32 nEventId,
            Object pEventParams
        ){
            bool bResult = false;
            switch( ( CALC_MODEL_EVENT )nEventId ){
            case CALC_MODEL_EVENT.DIGIT_0:{
                ;
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.DIGIT_1_9:{
                DIGIT_1_9_PARAMS e = ( DIGIT_1_9_PARAMS )pEventParams;
                BgnTrans( ZERO2, INT2 );
                set(e.digit);
                EndTrans( false );
                bResult = true;
            } break;
            case CALC_MODEL_EVENT.POINT:{
                BgnTrans( ZERO2, FRAC2 );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult ? bResult : Operand2_EventProc( nEventId, pEventParams );
        }
        void Zero2_Exit(){
            bool isThisLCA = Statemachine.IsIn( _stm.nSourceState, ZERO2 ) && Statemachine.IsIn( _stm.nTargetState, ZERO2 );
            if( !isThisLCA || _stm.bIsExternTrans ){ 
                _stm.bIsExternTrans &= !isThisLCA;
                Operand2_Exit();
            } else { 
                _stm.nLCAState = ZERO2;
            }
        }
        bool Start(
        ){
            bool bResult = false;
            switch( _stm.nCurrentState ){
            case STM:{
                BgnTrans( STM, ON );
                _calcView.DrawFrame();
                EndTrans( false );
                bResult = true;
            } break;
            case ON:{
                BgnTrans( STM, READY );
                EndTrans( false );
                bResult = true;
            } break;
            case READY:{
                BgnTrans( STM, BEGIN );
                EndTrans( false );
                bResult = true;
            } break;
            default: break;
            }
            return bResult;
        }
        public void Ac(
        ) {
            Object pEventParams = null;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.AC, pEventParams );
            RunToCompletion();
        }
        public void Ce(
        ) {
            Object pEventParams = null;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.CE, pEventParams );
            RunToCompletion();
        }
        public void Digit0(
        ) {
            Object pEventParams = null;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.DIGIT_0, pEventParams );
            RunToCompletion();
        }
        public void Digit19(
            char digit          
        ) {
            Object pEventParams = null;
            DIGIT_1_9_PARAMS eventParams = new DIGIT_1_9_PARAMS();
            pEventParams = ( Object )eventParams;
            eventParams.digit = digit;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.DIGIT_1_9, pEventParams );
            RunToCompletion();
        }
        public void Equals(
        ) {
            Object pEventParams = null;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.EQUALS, pEventParams );
            RunToCompletion();
        }
        public void Off(
        ) {
            Object pEventParams = null;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.OFF, pEventParams );
            RunToCompletion();
        }
        public void Oper(
            char key            
        ) {
            Object pEventParams = null;
            OPER_PARAMS eventParams = new OPER_PARAMS();
            pEventParams = ( Object )eventParams;
            eventParams.key = key;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.OPER, pEventParams );
            RunToCompletion();
        }
        public void Point(
        ) {
            Object pEventParams = null;
            EventProc( ( UInt32 )CALC_MODEL_EVENT.POINT, pEventParams );
            RunToCompletion();
        }
        bool Done(
        ){
            bool bResult = false;
            return bResult;
        }

    }
}
