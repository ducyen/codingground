using System;
namespace Main {
    class Statemachine : System.Object
    {
        Object _pParentContext;                                                                                                 /*<  */
        UInt32 _nCurrentState;                                                                                                  /*<  */
        UInt32 _nLCAState;                                                                                                      /*<  */
        UInt32 _nTargetState;                                                                                                   /*<  */
        UInt32 _nSourceState;                                                                                                   /*<  */
        bool _bIsFinished;                                                                                                      /*<  */
        bool _bIsExternTrans;                                                                                                   /*<  */
        public Statemachine (
            Object pParentContext
        ) {
            _pParentContext = pParentContext;                                                                                   /*<  */
            _nCurrentState = UInt32.MaxValue;                                                                                   /*<  */
            _nLCAState = 0;                                                                                                     /*<  */
            _nTargetState = 0;                                                                                                  /*<  */
            _nSourceState = 0;                                                                                                  /*<  */
            _bIsFinished = false;                                                                                               /*<  */
            _bIsExternTrans = false;                                                                                            /*<  */
        }
        public static bool IsIn( UInt32 subState,  UInt32 superState) {
            return ( superState >= subState && ( superState & subState ) > 0 );
        } // IsIn
        public static bool IsComposite( UInt32 x) {
            return !( ( ( x ) & ( ( x ) - 1 ) ) == 0 );
        } // IsComposite
        public Object pParentContext {
            get {
                return _pParentContext;
            }
            set {
                _pParentContext = value;
            }
        }
        public UInt32 nCurrentState {
            get {
                return _nCurrentState;
            }
            set {
                _nCurrentState = value;
            }
        }
        public UInt32 nLCAState {
            get {
                return _nLCAState;
            }
            set {
                _nLCAState = value;
            }
        }
        public UInt32 nTargetState {
            get {
                return _nTargetState;
            }
            set {
                _nTargetState = value;
            }
        }
        public UInt32 nSourceState {
            get {
                return _nSourceState;
            }
            set {
                _nSourceState = value;
            }
        }
        public bool bIsFinished {
            get {
                return _bIsFinished;
            }
            set {
                _bIsFinished = value;
            }
        }
        public bool bIsExternTrans {
            get {
                return _bIsExternTrans;
            }
            set {
                _bIsExternTrans = value;
            }
        }

    }
}
