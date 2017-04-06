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
    /// Returns one of two expressions depending on the value 
    /// of a boolean condition.
    /// </summary>
    /// <author>JoaoPe</author>
    public sealed class ConditionalOperator<TResult> : CodeActivity<TResult>
    {
        /// <summary>
        /// Gets or sets the condition to be evaluated.
        /// </summary>
        [RequiredArgument]
        public InArgument<bool> Condition { get; set; }

        /// <summary>
        /// Gets or sets the expression to be returned if condition evaluates to true.
        /// </summary>
        [RequiredArgument]
        public InArgument<TResult> FirstExpression { get; set; }

        /// <summary>
        /// Gets or sets the expression to be returned if condition evaluates to false.
        /// </summary>
        [RequiredArgument]
        public InArgument<TResult> SecondExpression { get; set; }

        protected override TResult Execute(CodeActivityContext context)
        {
            return Condition.Get<bool>(context) ?
                FirstExpression.Get<TResult>(context) :
                SecondExpression.Get<TResult>(context);
        }
    }
}