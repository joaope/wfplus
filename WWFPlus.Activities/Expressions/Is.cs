#region License
// Copyright (C) 2011 by João Correia (http://wwfplus.codeplex.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

namespace WWFPlus.Activities.Expressions
{
    using System.Activities;

    /// <summary>
    /// Checks if an object is compatible with a given type.
    /// </summary>
    /// <remarks>
    /// It works exactly the same way as C# <i>is</i> keyword.
    /// 
    /// Evaluates to <b>true</b> if the provided expression is non-null, and the provided
    /// Operand object can be cast to the provided type without causing an exception
    /// to be thrown.
    /// 
    /// Because it makes use of <i>is</i> keyword, it only considers reference conversions,
    /// boxing conversions, and unboxing conversions. Other conversions, such as user-defined
    /// conversions, are not considered.
    /// </remarks>
    /// <author>JoaoPe</author>
    public sealed class Is<TOperand, TType> : CodeActivity<bool>
    {
        /// <summary>
        /// Object to be checked against TType.
        /// </summary>
        [RequiredArgument]
        public InArgument<TOperand> Operand { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            return Operand.Get<TOperand>(context) is TType;
        }
    }
}