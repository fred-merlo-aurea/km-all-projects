using System;


namespace CssConvert.CssParser
{
    public enum Combinator
    {
        /// <summary></summary>
        ChildOf,				// >
        /// <summary></summary>
        PrecededImmediatelyBy,	// +
        /// <summary></summary>
        PrecededBy				// ~
    }
}